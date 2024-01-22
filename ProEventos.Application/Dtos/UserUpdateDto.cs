using ProEventos.Domain.Enum;
using ProEventos.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Dtos
{
    public class UserUpdateDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Titulo { get; set; }
        public string Description { get; set; } = "";
        public string Funcao { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string Password { get; set; } = "";
        public string Token { get; set; }
    }
}
