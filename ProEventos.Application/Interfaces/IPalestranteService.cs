using ProEventos.Application.Dtos;
using ProEventos.Domain.Model;
using ProEventos.Persistence.Pagination;
using System.Threading.Tasks;

namespace ProEventos.Application.Interfaces
{
    public interface IPalestranteService
    {
         Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto model);
         Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model);
         Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId,  bool includeEventos = false);
         Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false); 
    }
}