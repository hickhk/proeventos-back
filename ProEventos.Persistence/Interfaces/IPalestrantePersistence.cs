using System.Threading.Tasks;
using ProEventos.Domain.Model;
using ProEventos.Persistence.Interfaces;
using ProEventos.Persistence.Pagination;

namespace ProEventos.Application.Interfaces
{
    public interface IPalestrantePersistence: IGeralPersistence
    {
         Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEvento = false);
        Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEvento = false);
    }
}