using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Application;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Pagination;
using ProEventos_Api.Extensions;


namespace ProEventos_Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PalestrantesController : ControllerBase
    {
        private readonly ILogger<PalestrantesController> _logger;
        private readonly IPalestranteService _palestranteservice;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAccountService _accountService;

        public PalestrantesController(
                                 ILogger<PalestrantesController> logger, 
                                 IPalestranteService palestranteservice, 
                                 IWebHostEnvironment webHostEnvironment,
                                 IAccountService accountService
                                )
        {
            _logger = logger;
            _palestranteservice = palestranteservice;
            _webHostEnvironment = webHostEnvironment;
            _accountService = accountService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PageParams pageParams)
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsynk(userName);

                var palestrantes = await _palestranteservice.GetAllPalestrantesAsync(pageParams, true);
                if (palestrantes == null) return NoContent();

                Response.AddPagination(palestrantes.CurrentPage, palestrantes.PageSize, palestrantes.TotalCount, palestrantes.TotalPages);

                return Ok(palestrantes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Palestrantes. Erro: {ex.Message}");
            }
        }

        [HttpGet("getPalestrante")]
        public async Task<IActionResult> GetPalestranteAsync()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsynk(userName);

                if (user == null)
                    return null;

                var Palestrante = await _palestranteservice.GetPalestranteByUserIdAsync(user.UserId);
                if (Palestrante == null) return NoContent();

                return Ok(Palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Palestrante por id. Erro: {ex.Message}");
            }
        }



        [HttpPost]
        public async Task<IActionResult> PostAsync(PalestranteAddDto model)
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsynk(userName);

                model.UserId = user.UserId;

                var palestrante = await _palestranteservice.GetPalestranteByUserIdAsync(user.UserId);

                if (palestrante == null)
                {
                    palestrante = await _palestranteservice.AddPalestrante(user.UserId, model);
                    if (palestrante == null) return NoContent();
                }

                return Ok(new { palestrante });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar adicionar Palestrante. Erro: {ex.Message}");
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDto model)
        {
            try
            {

                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsynk(userName);

                var palestrante = await _palestranteservice.GetPalestranteByUserIdAsync(user.UserId);

                model.UserId = user.UserId;
                model.Id = palestrante.Id;

                if (palestrante != null)
                {
                    palestrante = await _palestranteservice.UpdatePalestrante(user.UserId, model);
                    if (palestrante == null) return NoContent();
                }

                return Ok(new { palestrante });
               
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar Palestrante. Erro: {ex.Message}");
            }
        }

    }
}
