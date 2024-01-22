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
using ProEventos.Persistence;
using ProEventos.Persistence.Contextos;


namespace ProEventos_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILogger<LotesController> _logger;
        private readonly ILoteService _loteService;

        public LotesController(ILogger<LotesController> logger, ILoteService loteService)
        {
            _logger = logger;
            _loteService = loteService;
        }


        [HttpGet("GetLotesByEventoId/{eventoId}")]
        public async Task<IActionResult> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _loteService.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar lotes. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetLoteByEventoIdLoteId/{eventoId}/{loteId}")]
        public async Task<IActionResult> GetLoteByEventoIdLoteIdAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteService.GetLoteByEventoIdLoteIdAsync(eventoId, loteId);
                if (lote == null) return NoContent();

                return Ok(lote);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar lotes. Erro: {ex.Message}");
            }
        }

        [HttpPut("SaveLotes/{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _loteService.SaveLote(eventoId, models);
                if (lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar lotes. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteService.GetLoteByEventoIdLoteIdAsync(eventoId, loteId);
                if (lote == null) return BadRequest("Erro ao tentar recuperar o lote");

                return await _loteService.DeleteLote(lote.EventoId, lote.Id)
                    ? Ok(new { message = "Lote Deletado" })
                    : this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar os lote. Erro:");

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar os lotes. Erro: {ex.Message}");
            }
        }
    }
}
