using Mapster;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Domain.Repositories.Recipe;
using myRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.Register
{
    public class RegisterRecipeUseCase : IRegisterRecipeUseCase
    {
        private readonly IRecipeWriteOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;
        public RegisterRecipeUseCase(
             IRecipeWriteOnlyRepository repository,
             ILoggedUser loggedUser,
             IUnitOfWork unitOfWork
            )
        {

            _repository = repository;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegistredRecipeJson> Execute(RequestRecipeJson request)
        {
            Validate(request);

            var recipe = request.Adapt<Domain.Entities.Recipe>();

            //MapsterConfiguration no projeto de Application
            //Ingredients e DishTypes , são enum, eles tem que serem convertidos para uma entidade 
            // o Mapster tem que receber uma configuração especifica par esta finalidade

            recipe.UserId = _loggedUser.GetUserId();

            await _repository.Add(recipe);

            await _unitOfWork.Commit();

            return new ResponseRegistredRecipeJson
            {
                Id = recipe.Id,
                Title = recipe.Title
            };

       }

        public static void Validate(RequestRecipeJson request)
        {
            var result = new RecipeValidator().Validate(request);
               
           if(result.IsValid==false)
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }

    }
}
