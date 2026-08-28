using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myRecipeBook.Application.UseCases.Recipe.DeleteById;
using myRecipeBook.Application.UseCases.Recipe.GetById;
using myRecipeBook.Application.UseCases.Recipe.Register;
using myRecipeBook.Application.UseCases.User.Register;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;

namespace myRecipeBook.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class RecipesController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegistredRecipeJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
                                    [FromBody] RequestRecipeJson request,
                                    [FromServices] IRegisterRecipeUseCase useCase)
        {
            var result = await useCase.Execute(request);
            // registra a receita 
            return Created(string.Empty, result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
                                [FromRoute] Guid id,
                                [FromServices] IGetRecipeByIdUseCase useCase)
        {
            var recipe = await useCase.Execute(id);
            return Ok(recipe);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
                          [FromRoute] Guid id,
                          [FromServices] IDeleteRecipeByIdUseCase useCase)
        {
            await useCase.Execute(id);
            return NoContent();

        }
    }
}
