using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myRecipeBook.Application.UseCases.Recipe.DeleteById;
using myRecipeBook.Application.UseCases.Recipe.Filters;
using myRecipeBook.Application.UseCases.Recipe.GetById;
using myRecipeBook.Application.UseCases.Recipe.Recent;
using myRecipeBook.Application.UseCases.Recipe.Register;
using myRecipeBook.Application.UseCases.Recipe.UpdateById;
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
        
        // a ordem dos endpoints sao importantes
        // a exemplo deste GET(id)  quando esta nomeado vem por ultimo 
        
        [HttpGet("recent")]
        [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecent([FromServices] IGetRecentRecipesUseCase useCase)
        {
            var recipes = await useCase.Execute();
            return Ok(recipes);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(RequestFilterRecipesJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> Filter(
            [FromServices] IFilterRecipesUseCase useCase,
            [FromBody] RequestFilterRecipesJson request)
        {
            var response = await useCase.Execute(request);
            return Ok(response);
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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
                       [FromRoute] Guid id,
                       [FromBody] RequestRecipeJson request,
                       [FromServices] IUpdateRecipeByIdUseCase useCase)
        {
            await useCase.Execute(id, request);
            return NoContent();

        }

    
    }
}