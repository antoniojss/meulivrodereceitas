using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myRecipeBook.Application.UseCases.User.ChangePassword;
using myRecipeBook.Application.UseCases.User.Profile;
using myRecipeBook.Application.UseCases.User.Register;
using myRecipeBook.Application.UseCases.User.Update;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Responses;

namespace myRecipeBook.API.Controllers
{
    /* este formato coloca o nome API no sweeger  api/users
    [Route("api/[controller]")]
    */

    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /*  não utilizar assim por que se não vai trazer as classes que não vai ser 
         *  usado o modo exemplifocado mais abaixo 
        private readonly IRegisteruserAccountUseCase _registerUserAccountUseCase;
        public UsersController(IRegisteruserAccountUseCase registerUserAccountUseCase)
        {
            _registerUserAccountUseCase = registerUserAccountUseCase;
        }   
        */


        [HttpPost]


        //FromBody - da onde vai pegar 
        //RequestRegisterUserAccountJson e a classe que montamos para conter as informações dos usuarios
        //Post e o metodo que indica o que vai ser feito 
        //ProducesResponseType usado para mostrar as mensagens de erro no swegger 
        [ProducesResponseType(typeof(ResponseRegistredUserJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task< IActionResult> Register(
                                      [FromBody]RequestRegisterUserAccountJson request, 
                                      [FromServices] IRegisterUserAccountUseCase useCase )
        {
           var result = await useCase.Execute(request);
            // registra a conta do usuario
            return Created(string.Empty, result);
        }


        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
        {
            var result = await useCase.Execute();
            return Ok(result);
        }

        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProfile(
                    [FromServices] IUpdateUserUseCase useCase,
                    [FromBody] RequestUpdateUserJson request)
        {
            await useCase.Execute(request);
            return NoContent();
        }

        [HttpPut("password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePassword(
            [FromServices] IChangePasswordUseCase useCase,
            [FromBody] RequestChangePasswordJson request)
        {
            await useCase.Execute(request);
            return NoContent();
        }
    }
}
