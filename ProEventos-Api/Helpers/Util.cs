using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Helpers.Interfaces;
using ProEventos_Api.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos_Api.Helpers.Helpers
{
    public class Util : IUtil
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Util(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        public void DeleteImage(string imageName, string destination)
        {
            var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, @$"Resources/{destination}", imageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        public async Task<string> SaveImage(IFormFile imageFile, string destination)
        {
            var imagePath = "";
            var imageName = "";
            try
            {
                imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(20).ToArray()).Replace(" ", "-");

                imageName = $"{imagePath}{DateTime.UtcNow.ToString("ssfff")}{Path.GetExtension(imageFile.FileName)}";

                imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, @$"Resources/{destination}", imageName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
            }
            catch (Exception error)
            {
                string errorMessage = error.Message;
                throw;
            }

            return imageName;
        }
    }
}
