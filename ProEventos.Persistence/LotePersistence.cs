using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Model;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Persistence
{
    public class LotePersistence : ILotePersistence
    {
        private readonly ProEventoContext _context;

        public LotePersistence(ProEventoContext context)
        {
            _context = context;
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.Where(lote => lote.EventoId == eventoId).AsNoTracking();

            return await query.ToArrayAsync();
        }

        public async Task<Lote> GetLoteByEventoIdLoteIdAsync(int eventoId, int lotId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.Where(lote => lote.EventoId == eventoId && lote.Id == lotId).AsNoTracking();

            return await query.FirstOrDefaultAsync();

        }
    }
}