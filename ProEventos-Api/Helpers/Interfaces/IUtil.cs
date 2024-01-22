using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos_Api.Helpers.Interfaces
{
    public interface IUtil
    {
        Task<string> SaveImage(IFormFile imageFile, string destination);
        void DeleteImage(string imageName, string destination);
    }
}
