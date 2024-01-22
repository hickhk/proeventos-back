using ProEventos.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Dtos
{
    public class PalestranteAddDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MiniCurriculo { get; set; }


    }
}
