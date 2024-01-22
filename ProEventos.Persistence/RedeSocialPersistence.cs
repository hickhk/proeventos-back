using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProEventos.Domain.Model;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class RedeSocialPersistence: GeralPersistence, IRedeSocialPersistence
    {
        private readonly ProEventoContext _context;

        public RedeSocialPersistence(ProEventoContext proEventoContext) : base(proEventoContext)
        {
            _context = proEventoContext;
        }
        public async Task<RedeSocial> GetRedeSocialEventoByIdAsync(int eventoId, int id)
        {
            IQueryable<RedeSocial> query = _context.RedeSocial
                .Where(rs => rs.EventoId == eventoId && rs.Id == id)
                .AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }
        public async Task<RedeSocial> GetRedeSocialPalestranteByIdAsync(int palestranteId, int id)
        {
            IQueryable<RedeSocial> query = _context.RedeSocial
                .Where(rs => rs.PalestranteId == palestranteId && rs.Id == id)
                .AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }
        public async Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId)
        {
            IQueryable<RedeSocial> query = _context.RedeSocial
                .Where(rs => rs.EventoId == eventoId)
                .AsNoTracking();

            return await query.ToArrayAsync();
        }
        public async Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            IQueryable<RedeSocial> query = _context.RedeSocial
               .Where(rs => rs.PalestranteId == palestranteId)
               .AsNoTracking();

            return await query.ToArrayAsync();
        }

    }
}
