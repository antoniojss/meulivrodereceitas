using FluentValidation.Results;
using Mapster;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Domain.Repositories.User;
using myRecipeBook.Domain.Security.PasswordHashing;
using myRecipeBook.Domain.Security.Tokens;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;

namespace myRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserAccountUseCase : IRegisterUserAccountUseCase
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public RegisterUserAccountUseCase(
            IPasswordHasher passwordHasher,
            IUserWriteOnlyRepository userWriteOnlyRepository,
            IUserReadOnlyRepository userReadOnlyRepository,
            IUnitOfWork unitOfWork,
            IAccessTokenGenerator accessTokenGenerator)
        {
            _passwordHasher = passwordHasher;
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<ResponseRegistredUserJson> Execute(RequestRegisterUserAccountJson request)
        {
           await ValidateAndThrowOnFailures(request);

            //usar a bribiloteca de mapeamento automatico da entidade 
            //Mapster

            var user = request.Adapt<Domain.Entities.User>();

            user.Password = _passwordHasher.HashPassword(request.Password);

            await _userWriteOnlyRepository.Add(user);

            await _unitOfWork.Commit();

            return new ResponseRegistredUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user)
                }

            };

        }
        private async Task ValidateAndThrowOnFailures(RequestRegisterUserAccountJson request)
        {

            // Implementation of user validation and registration logic goes here
            var validator = new RegisterUserAccountValidator();
            var result = validator.Validate(request);


            var emailExists = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if(emailExists)
            {
                 result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS));   
            }  
            
            // tratar a lista de erros que de exceptions  
            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
