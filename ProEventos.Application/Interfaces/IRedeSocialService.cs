using ProEventos.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Interfaces
{
    public interface IRedeSocialService
    {
       Task<RedeSocialDto[]> SaveRedeSocialByEvento(int eventoId,  RedeSocialDto[] models);
        Task<RedeSocialDto[]> SaveRedeSocialByPalestrante(int palestranteId, RedeSocialDto[] models);
        Task<bool> DeleteRedeSocialByEvento(int eventoId, int RedeSocialId);
        Task<bool> DeleteRedeSocialByPalestante(int palestranteId, int RedeSocialId);
        Task<RedeSocialDto[]>  GetAllRedeSocialByEventoIdAsync(int eventoId );
       Task<RedeSocialDto[]>  GetAllRedeSocialByPalestranteIdAsync(int palestranteId );
       Task<RedeSocialDto>  GetAllRedeSocialEventoByIdAsync(int eventoId, int redeSocialId);
       Task<RedeSocialDto> GetAllRedeSocialPalestranteByIdAsync(int palestranteId, int redeSocialId);
    }
}
