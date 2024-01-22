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
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Persistence;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Pagination;
using ProEventos_Api.Extensions;
using ProEventos_Api.Helpers.Interfaces;


namespace ProEventos_Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly ILogger<EventosController> _logger;
        private readonly IEventoService _eventoService;
        private readonly IUtil _util;
        private readonly IAccountService _accountService;
        private readonly string _destination = "images";

        public EventosController(
                                 ILogger<EventosController> logger, 
                                 IEventoService eventoService,
                                 IUtil util,
                                 IAccountService accountService
                                )
        {
            _logger = logger;
            _eventoService = eventoService;
            _util = util;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]PageParams pageParams)
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsynk(userName);

                var eventos = await _eventoService.GetAllEventosAsync(user.UserId, pageParams, true);
                if (eventos == null) return NoContent();

                Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,$"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }
         [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUseId(), id,true);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento por id. Erro: {ex.Message}");
            }
        }


        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUseId(), eventoId, false);
                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    _util.DeleteImage(evento.ImagemUrl, _destination);

                    evento.ImagemUrl = await _util.SaveImage(file, _destination);
                }

                var eventoRetorno = await _eventoService.UpdateEvento(User.GetUseId(), eventoId, evento);

                return Ok(new { evento });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar adicionar evento. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(EventoDto model)
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsynk(userName);

                model.UserId = user.UserId;

                var evento =  await _eventoService.AddEvento(user.UserId, model);
                if (evento == null) return NoContent();

                return Ok(new { evento });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar adicionar evento. Erro: {ex.Message}");
            }
        }
         [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var result = await _eventoService.UpdateEvento(User.GetUseId(), id, model);
                if (result == null) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar evento. Erro: {ex.Message}");
            }
        }
         [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUseId(), id, true);

                if (await _eventoService.DeleteEvento(User.GetUseId(), id)){
                    _util.DeleteImage(evento.ImagemUrl, _destination);
                    return Ok(new {message = "Deletado"});

                }
                else
                {
                    return BadRequest("Erro ao tentar deletar o evento");

                }


            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar evento. Erro: {ex.Message}");
            }
        }

    }
}
