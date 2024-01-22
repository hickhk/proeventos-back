using ProEventos.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Dtos
{
    public class RedeSocialDto
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Url { get; set; }
        public int? EventoId { get; set; }
        public EventoDto Evento { get; set; }
        public int? PalestranteId { get; set; }
        public PalestranteDto Palestrante { get; set; }
    }
}
