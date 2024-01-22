using ProEventos.Application.Dtos;
using ProEventos.Domain.Model;
using ProEventos.Persistence.Pagination;
using System.Threading.Tasks;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
         Task<EventoDto> AddEvento(int userId, EventoDto model);
         Task<EventoDto> UpdateEvento(int userId, int id, EventoDto model);
         Task<bool> DeleteEvento(int userId, int id);
         Task<EventoDto> GetEventoByIdAsync(int userId, int id, bool includePalestrantes);
         Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes);
    }
}