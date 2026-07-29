using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi;
using myRecipeBook.Communication.Responses;
using myRecipeBook.Exception;
using myRecipeBook.Exception.ExceptionBase;

namespace myRecipeBook.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is MyRecipeBookException myRecipeBookException)
            {
                context.HttpContext.Response.StatusCode = (int)myRecipeBookException.GetStatusCode();
                context.Result = new ObjectResult(new ResponseErrorJson(myRecipeBookException.GetErrorMessages()));
            }
            else
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
            }
        }
    }
}
