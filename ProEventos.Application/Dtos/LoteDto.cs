using ProEventos.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public decimal Preco { get; set; }
        
        [Required]
        public string DataInicio { get; set; }

        [Required]
        public string DataFim { get; set; }

        [Required]
        public int Quantidade { get; set; }
        public int EventoId { get; set; }
       // public EventoDto Evento { get; set; }
    }
}
