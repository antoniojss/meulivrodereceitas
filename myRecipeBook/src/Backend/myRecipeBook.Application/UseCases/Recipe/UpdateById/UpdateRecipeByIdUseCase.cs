using Mapster;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories;
using myRecipeBook.Domain.Repositories.Recipe;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.UpdateById
{
    public class UpdateRecipeByIdUseCase : IUpdateRecipeByIdUseCase
    {
        private readonly IRecipeUpdateOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateRecipeByIdUseCase(
             IRecipeUpdateOnlyRepository repository,
             ILoggedUser loggedUser,
             IUnitOfWork unitOfWork
            )
        {

            _repository = repository;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(Guid recipeId, RequestRecipeJson request)
        {
            ValidateAndThrowOnFailures(request);

            var recipe = await _repository.GetById(recipeId, _loggedUser.GetUserId());

            if (recipe is null)
                throw new NotFoundException(ResourceMessagesException.VALIDATION_INVALID_RECIPE_NOT_FOUND);

            //apaga e reconstrói a receita com os novos dados, para evitar problemas de atualização de coleções
            request.Adapt(recipe);

            await _unitOfWork.Commit();
        }

        public static void ValidateAndThrowOnFailures(RequestRecipeJson request)
        {
            var result = new RecipeValidator().Validate(request);

            if (result.IsValid == false)
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}

