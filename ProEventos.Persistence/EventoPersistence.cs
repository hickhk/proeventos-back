using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Model;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Interfaces;
using ProEventos.Persistence.Pagination;

namespace ProEventos.Persistence
{
    public class EventoPersistence : IEventoPersistence
    {
        private readonly ProEventoContext _context;

        public EventoPersistence(ProEventoContext context)
        {
            _context = context;
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        
        public async Task<Evento> GetEventoByIdAsync(int userId,  int id, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.AsNoTracking()
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais).Where(ev => ev.Id == id );

            query.OrderBy(e => e.Id);

            if(includePalestrantes){
              query
              .Include(e => e.PalestrantesEventos)
              .ThenInclude(e => e.Palestrante);
            }
            
            query.Where(ev => ev.Id == id && ev.UserId == userId);

            return await query.FirstAsync();
        }

        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.AsNoTracking()
                .Where(
                        ev => (
                               ev.Tema.ToLower().Contains(pageParams.Term.ToLower()) || 
                               ev.Local.ToLower().Contains(pageParams.Term.ToLower())
                               ) && ev.UserId == userId)
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            query.OrderBy(e => e.Id);

            if(includePalestrantes){
              query
              .Include(e => e.PalestrantesEventos)
              .ThenInclude(e => e.Palestrante);
            }

            var result = await PageList<Evento>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);

            return result;
             
        }
    }
}