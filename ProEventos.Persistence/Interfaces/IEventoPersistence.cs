using System.Threading.Tasks;
using ProEventos.Domain.Model;
using ProEventos.Persistence.Pagination;

namespace ProEventos.Persistence.Interfaces
{
    public interface IEventoPersistence
    {
         Task<Evento> GetEventoByIdAsync(int userId,  int id, bool includePalestrantes);
         
         Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes);
    }
}