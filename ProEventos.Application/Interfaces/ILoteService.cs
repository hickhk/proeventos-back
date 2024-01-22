using ProEventos.Application.Dtos;
using ProEventos.Domain.Model;
using System.Threading.Tasks;

namespace ProEventos.Application.Interfaces
{
    public interface ILoteService
    {
        Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId);
        Task<LoteDto> GetLoteByEventoIdLoteIdAsync(int eventoId, int loteId);
        Task<LoteDto[]> SaveLote(int eventoId, LoteDto[] models);
        Task<bool> DeleteLote(int eventoId, int loteId);
    }
}