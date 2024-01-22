using IdentityModel;
using Microsoft.AspNetCore.Identity;
using ProEventos.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Titulo Titulo { get; set; }
        public string Description { get; set; }
        public Funcao Funcao { get; set; }
        public string Telefone { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
        

        public string FullName
        {
            get { return FirstName + LastName; }
        }

    }
}
