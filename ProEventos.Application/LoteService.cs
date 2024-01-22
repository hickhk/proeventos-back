using AutoMapper;
using Microsoft.Extensions.Logging;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Model;
using ProEventos.Persistence;
using ProEventos.Persistence.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class LoteService : ILoteService
    {
        private readonly IGeralPersistence _geralPersistence;
        private readonly ILotePersistence _lotePersistence;
        private readonly IMapper _mapper;

        public LoteService(IGeralPersistence geralPersistence, ILotePersistence lotePersistence, IMapper mapper)
        {
            _geralPersistence = geralPersistence;
            _lotePersistence = lotePersistence;
            _mapper = mapper;
        }

        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotePersistence.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;

                return _mapper.Map <LoteDto[]>(lotes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto> GetLoteByEventoIdLoteIdAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersistence.GetLoteByEventoIdLoteIdAsync(eventoId, loteId);
                if (lote == null) return null;

                return _mapper.Map<LoteDto>(lote);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto[]> SaveLote(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotePersistence.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;

                foreach (var model in models)
                {
                    model.EventoId = eventoId;

                    if (model.Id == 0)
                    {
                        await AddLote(model);
                    }
                    else
                    {
                        await UpdateLote(model);
                    }
                }
                var loteRetorno = await _lotePersistence.GetLotesByEventoIdAsync(eventoId);
                return _mapper.Map<LoteDto[]>(loteRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersistence.GetLoteByEventoIdLoteIdAsync(eventoId, loteId);

                if (lote == null) throw new Exception("O Lote não foi encontrado para deletar!");

                _geralPersistence.Delete(lote);
                return await _geralPersistence.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateLote(LoteDto model)
        {
            try
            {
                var loteModel = _mapper.Map<Lote>(model);
                _geralPersistence.Update<Lote>(loteModel);
                await _geralPersistence.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddLote(LoteDto model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                _geralPersistence.Add<Lote>(lote);
                await _geralPersistence.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}