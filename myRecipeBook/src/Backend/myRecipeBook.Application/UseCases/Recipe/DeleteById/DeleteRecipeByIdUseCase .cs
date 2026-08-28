using myRecipeBook.Application.UseCases.Recipe.GetById;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Repositories.Recipe;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.DeleteById
{
    public class DeleteRecipeByIdUseCase : IDeleteRecipeByIdUseCase
    {
        private readonly IRecipeWriteOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;

        public DeleteRecipeByIdUseCase(IRecipeWriteOnlyRepository repository, ILoggedUser loggedUser)
        {
            _repository = repository;
            _loggedUser = loggedUser;
        }

        public async Task Execute(Guid recipeId)
        {
            var deleted = await _repository.DeleteById(recipeId, _loggedUser.GetUserId());
            if (deleted == false)
                throw new NotFoundException(ResourceMessagesException.VALIDATION_INVALID_RECIPE_NOT_FOUND);
        }
    }
}