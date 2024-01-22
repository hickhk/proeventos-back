using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Model;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Pagination;

namespace ProEventos.Persistence
{
    public class PalestrantePersistence : GeralPersistence, IPalestrantePersistence
    {
        private readonly ProEventoContext _context;

        public PalestrantePersistence(ProEventoContext proEventoContext): base(proEventoContext)
        {
            _context = proEventoContext;
        }

        public async Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEvento = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Where(
                p => p.UserId == userId).Include(p => p.User).Include(p => p.RedesSociais).AsNoTracking();

                query.OrderBy(p => p.Id);

                if(includeEvento){
                  query
                  .Include(p => p.PalestrantesEventos)
                  .ThenInclude(p => p.Evento);
                }

            return query.FirstOrDefault();
        }

        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEvento = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Where(
                p => (
                p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) ||
                p.User.FirstName.ToLower().Contains(pageParams.Term.ToLower()) ||
                p.User.LastName.ToLower().Contains(pageParams.Term.ToLower()) &&
                p.User.Funcao == Domain.Enum.Funcao.Palestrante)
                ).AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.RedesSociais)
                .OrderBy(p => p.Id);

            if(includeEvento){
              query
              .Include(p => p.PalestrantesEventos)
              .ThenInclude(p => p.Evento);
            }

            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);
        }

    }
}