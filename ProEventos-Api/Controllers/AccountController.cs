using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos_Api.Extensions;
using ProEventos_Api.Helpers.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProEventos_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly IUtil _util;
        private readonly string _destination = "images/users";

        public AccountController(
                                 IAccountService accountService,
                                 ITokenService tokenService,
                                 IUtil util
                                )
        {
            _accountService = accountService;
            _tokenService = tokenService;
            _util = util;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();

                var user = await _accountService.GetUserByUserNameAsynk(userName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Usuário. Erro: {ex.Message}");
            }
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsynk(userLogin.UserName);
                if (user == null)
                    return Unauthorized("Usuário e/ou senha inválidos!");

                var result = await _accountService.CheckUserPasswordAsync(user, userLogin.Password);
                if(!result.Succeeded)
                    return Unauthorized("Usuário e/ou senha inválidos!");

                return Ok(
                    new
                    {
                        userId = user.UserId,
                        userName = user.UserName,
                        Nome = user.FirstName +" "+ user.LastName,
                        token = _tokenService.CreateToken(user).Result
                    }
                    );
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("RegisterUser")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser( UserUpdateDto userDto)
        {
            try
            {
                
                if(await _accountService.UserExists(userDto.UserName))
                    return BadRequest("O usuário " + userDto.UserName + " já existe.");

                var user = await _accountService.CreateAccountAsync(userDto);
                if(user != null)

                    return Ok
                        (
                          new
                          {
                              userId = user.UserId,
                              userName = user.UserName,
                              Nome = user.FirstName + " " + user.LastName,
                              token = _tokenService.CreateToken(user).Result
                          }
                        );

                return BadRequest("O usuário " + userDto.UserName + " não criado, tente mais tarde.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar registrar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdate)
        {
            try
            {

                if(userUpdate.UserName != User.GetUserName())
                    return Unauthorized("Usuário inválido!");

                var user = await _accountService.GetUserByUserNameAsynk(User.GetUserName());
                if (user == null)
                    return Unauthorized("Usuário inválido!");

                userUpdate.UserId = user.UserId;

                var result = await _accountService.UpdateAccount(userUpdate);
                if (result == null)
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar Atualizar Usuário. Erro: {ex.Message}");
            }
        }
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsynk(User.GetUserName());
                if (user == null)
                    return Unauthorized("Usuário inválido!");




                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    _util.DeleteImage(user.ImageUrl, _destination);

                    user.ImageUrl = await _util.SaveImage(file, _destination);

                    var result = await _accountService.UpdateAccount(user);
                    if (result == null) return NoContent(); 
                }


                return Ok(new { user });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar adicionar evento. Erro: {ex.Message}");
            }
        }

    }
}
