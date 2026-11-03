using FluentValidation.Results;
using Mapster;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Domain.Repositories.RefreshToken;
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
        private readonly IRefreshTokenWriteOnlyRepository _refreshTokenWriteOnlyRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        
         private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        public RegisterUserAccountUseCase(
            IPasswordHasher passwordHasher,
            IUserWriteOnlyRepository userWriteOnlyRepository,
            IUserReadOnlyRepository userReadOnlyRepository,
            IUnitOfWork unitOfWork,
            IAccessTokenGenerator accessTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository)
        {
            _passwordHasher = passwordHasher;
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenWriteOnlyRepository = refreshTokenWriteOnlyRepository;
        }

        public async Task<ResponseRegistredUserJson> Execute(RequestRegisterUserAccountJson request)
        {
           await ValidateAndThrowOnFailures(request);

            //usar a bribiloteca de mapeamento automatico da entidade 
            //Mapster

            var user = request.Adapt<Domain.Entities.User>();

            user.Password = _passwordHasher.HashPassword(request.Password);

            await _userWriteOnlyRepository.Add(user);

            var refreshToken = await GenerateAndSaveRefreshToken(user.Id); 

            await _unitOfWork.Commit();

            return new ResponseRegistredUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user),
                    RefreshToken  = refreshToken
                }

            };

        }

        private async Task<string> GenerateAndSaveRefreshToken(Guid userId)
        {
            var refreshToken = new RefreshToken
                {
                    Value = _refreshTokenGenerator.GenerateToken(),  
                    UserId = userId,
                };          
            await _refreshTokenWriteOnlyRepository.Replace(refreshToken);
            return refreshToken.Value;
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
