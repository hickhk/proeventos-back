using ProEventos.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Interfaces
{
    public interface IRedeSocialPersistence: IGeralPersistence
    {
        Task<RedeSocial> GetRedeSocialEventoByIdAsync(int eventoId, int id);
        Task<RedeSocial> GetRedeSocialPalestranteByIdAsync(int palestranteId, int id);
        Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId);
    }
}
