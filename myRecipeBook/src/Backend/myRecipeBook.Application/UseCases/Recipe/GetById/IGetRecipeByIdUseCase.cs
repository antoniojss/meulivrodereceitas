using myRecipeBook.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.UseCases.Recipe.GetById;
public interface IGetRecipeByIdUseCase
{
    Task<ResponseRecipeJson> Execute(Guid recipeId);
}
