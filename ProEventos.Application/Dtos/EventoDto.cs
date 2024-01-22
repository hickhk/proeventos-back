using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
         public int Id { get; set; }
        public int UserId { get; set; }
        public string Local { get; set; }
        [Required]
        public string DataEvento { get; set; }
        [Required]
        public string Tema { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Telefone { get; set; }
        [Display(Name = "Qtd de Pessoas")]
        public int QtdPessoas { get; set; }
        public string ImagemUrl { get; set; }
        public IEnumerable<LoteDto> Lotes { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        public IEnumerable<PalestranteEventoDto> PalestrantesEventos { get; set; }
    }
}