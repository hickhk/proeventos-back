using ProEventos.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Dtos
{
    public class PalestranteDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MiniCurriculo { get; set; }
        public UserUpdateDto user { get; set; }
        public IEnumerable<RedeSocial> RedesSociais { get; set; }
        public IEnumerable<EventoDto> Eventos { get; set; }
    }
}
