using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using ProEventos.Application;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Model;
using ProEventos.Persistence;
using ProEventos.Persistence.Contextos;
using ProEventos_Api.Extensions;


namespace ProEventos_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedesSociaisController : ControllerBase
    {
        private readonly ILogger<RedesSociaisController> _logger;
        private readonly IEventoService _eventoService;
        private readonly IPalestrantePersistence _palestranteService;
        private readonly IRedeSocialService _redesSociaiservice;
        private readonly IAccountService _accountService;

        public RedesSociaisController(
            ILogger<RedesSociaisController> logger,
            IEventoService eventoService,
            IPalestrantePersistence palestranteService,
            IRedeSocialService redesSociaiservice,
            IAccountService accountService
            
            )
        {
            _logger = logger;
            _eventoService = eventoService;
            _palestranteService = palestranteService;
            _redesSociaiservice = redesSociaiservice;
            _accountService = accountService;
        }


        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetRedesSociaisByEventoIdAsync(int eventoId)
        {
            try
            {
                if (await AutorEvento(eventoId) == false)
                    return Unauthorized();

                var redesSociais = await _redesSociaiservice.GetAllRedeSocialByEventoIdAsync(eventoId);
                if (redesSociais == null) return NoContent();

                return Ok(redesSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar RedesSociais. Erro: {ex.Message}");
            }
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetRedesSociaisByPalestranteIdAsync()
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsynk(User.GetUserName());
                if (user == null) return NoContent();

                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(user.UserId);
                if (palestrante == null) return Unauthorized();


                var RedesSociais = await _redesSociaiservice.GetAllRedeSocialByPalestranteIdAsync(palestrante.Id);
                if (RedesSociais == null) return NoContent();

                return Ok(RedesSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar RedesSociais. Erro: {ex.Message}");
            }
        }


        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveRedeSociaiByEventoAsync(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                if (await AutorEvento(eventoId) == false)
                    return Unauthorized();

                var RedesSociais = await _redesSociaiservice.SaveRedeSocialByEvento(eventoId, models);
                if (RedesSociais == null) return NoContent();

                return Ok(RedesSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar RedesSociais. Erro: {ex.Message}");
            }
        }

        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveRedeSocialByPalestranteAsync(RedeSocialDto[] models)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsynk(User.GetUserName());
                if (user == null) return NoContent();

                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(user.UserId);
                if (palestrante == null) return Unauthorized();

                var RedesSociais = await _redesSociaiservice.SaveRedeSocialByPalestrante(palestrante.Id, models);
                if (RedesSociais == null) return NoContent();

                return Ok(RedesSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar RedesSociais. Erro: {ex.Message}");
            }
        }

        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> Delete(int eventoId, int redeSocialId)
        {
            try
            {
                if (await AutorEvento(eventoId) == false)
                    return Unauthorized();

                var RedesSociais = await _redesSociaiservice.GetAllRedeSocialEventoByIdAsync(eventoId, redeSocialId);
                if (RedesSociais == null) return NoContent();


                return await _redesSociaiservice.DeleteRedeSocialByEvento(eventoId, redeSocialId)
                    ? Ok(new { message = "Rede Social do evento Deletada." })
                    : this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar a Rede Social por evento. Erro:");

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar a Rede Social. Erro: {ex.Message}");
            }
        }

        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteRedeSocialByPalestranteAsync(int redeSocialId)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsynk(User.GetUserName());
                if (user == null) return NoContent();

                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(user.UserId);
                if (palestrante == null) return Unauthorized();

                var redeSocial = await _redesSociaiservice.GetAllRedeSocialPalestranteByIdAsync(palestrante.Id, redeSocialId);
                if (palestrante == null) return NoContent();

                return await _redesSociaiservice.DeleteRedeSocialByPalestante(palestrante.Id, redeSocialId)
                    ? Ok(new { message = "Rede Social Deletada do palestrante deletada." })
                    : this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar a Rede Social por palestrante. Erro:");

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar a Rede Social. Erro: {ex.Message}");
            }
        }

        [NonAction]
        private async Task<bool> AutorEvento(int eventoId)
        {
            var user = await _accountService.GetUserByUserNameAsynk(User.GetUserName());

            var evento = _eventoService.GetEventoByIdAsync(user.UserId, eventoId, false);
            if (evento == null) return false;

            return true;
        }
    }
}
