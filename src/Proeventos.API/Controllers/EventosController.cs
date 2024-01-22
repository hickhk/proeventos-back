using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proeventos.API.Data;
using Proeventos.API.Models;

namespace Proeventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly ILogger<EventosController> _logger;
        private readonly DataContext _context;

        public EventosController(ILogger<EventosController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _context.Eventos;
        }
         [HttpGet("{id}")]
        public Evento GetById(int id)
        {
            //return _context.Eventos.Where(evento => evento.EventoId == id);
             return _context.Eventos.FirstOrDefault(evento => evento.EventoId == id);
        }
        
        [HttpPost]
        public string Post()
        {
            return "Exemplo de post";
        }
         [HttpPut("{id}")]
        public string Put(int id)
        {
            return $"Exemplo de put {id}" ;
        }
         [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return $"Exemplo de delete {id}" ;
        }

    }
}
