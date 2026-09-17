using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myRecipeBook.Application.UseCases.Login.WithEmailAndPassword;
using myRecipeBook.Application.UseCases.PasswordRecovery.RequestCode;
using myRecipeBook.Application.UseCases.PasswordRecovery.ResetPassword;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;

namespace myRecipeBook.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseRegistredUserJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> Login(
            [FromServices] ILoginWithEmailAndPasswordUseCase useCase,
            [FromBody] RequestLoginJson request)
        {
            var response = await useCase.Execute(request);

            return Ok(response);
        }

        [HttpPost("password-recovery")]
        [ProducesResponseType(typeof(ResponseRegistredUserJson), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> PasswordRecovery(
            [FromServices] IRequestPasswordRecoveryCodeUseCase useCase,
            [FromBody] RequestPasswordRecoveryJson request)
        {
            await useCase.Execute(request);
            
            return Accepted();  
        }

        [HttpPost("password-recovery/reset")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseRegistredUserJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(
            [FromServices] IResetPasswordUseCase useCase,
            [FromBody] RequestResetPasswordJson request)
        {
            await useCase.Execute(request);
            
            return NoContent();  
        }
    }
}
