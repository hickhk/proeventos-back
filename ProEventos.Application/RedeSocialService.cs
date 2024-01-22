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
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IGeralPersistence _geralPersistence;
        private readonly IRedeSocialPersistence _redeSocialPersistence;
        private readonly IMapper _mapper;

        public RedeSocialService(IGeralPersistence geralPersistence, IRedeSocialPersistence redeSocialPersistence, IMapper mapper)
        {
            _redeSocialPersistence = redeSocialPersistence;
            _geralPersistence = geralPersistence;
            _mapper = mapper;
        }

        public async Task<RedeSocialDto[]> GetAllRedeSocialByEventoIdAsync(int eventoId)
        {
            try
            {
                var RedeSocials = await _redeSocialPersistence.GetAllByEventoIdAsync(eventoId);
                if (RedeSocials == null) return null;

                return _mapper.Map <RedeSocialDto[]>(RedeSocials);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllRedeSocialByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var RedeSocial = await _redeSocialPersistence.GetAllByPalestranteIdAsync(palestranteId);
                if (RedeSocial == null) return null;

                return _mapper.Map<RedeSocialDto[]>(RedeSocial);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetAllRedeSocialEventoByIdAsync(int eventoId = 0, int redeSocialId = 0)
        {
            try
            {
                var RedeSocial = await _redeSocialPersistence.GetRedeSocialEventoByIdAsync(eventoId, redeSocialId);
                if (RedeSocial == null) return null;

                return _mapper.Map<RedeSocialDto>(RedeSocial);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetAllRedeSocialPalestranteByIdAsync(int palestranteId = 0, int redeSocialId = 0)
        {
            try
            {
                var RedeSocial = await _redeSocialPersistence.GetRedeSocialPalestranteByIdAsync(palestranteId, redeSocialId);
                if (RedeSocial == null) return null;

                return _mapper.Map<RedeSocialDto>(RedeSocial);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<RedeSocialDto[]> SaveRedeSocialByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                var RedeSocials = await _redeSocialPersistence.GetAllByEventoIdAsync(eventoId);
                if (RedeSocials == null) return null;

                foreach (var model in models)
                {
                    model.EventoId = eventoId;

                    if (model.Id == 0)
                    {
                        await AddRedeSocial(eventoId, 0, model);
                    }
                    else
                    {
                        await UpdateRedeSocial(model);
                    }
                }
                var RedeSocialRetorno = await _redeSocialPersistence.GetAllByEventoIdAsync(eventoId);
                return _mapper.Map<RedeSocialDto[]>(RedeSocialRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SaveRedeSocialByPalestrante(int palestranteId, RedeSocialDto[] models)
        {
            try
            { 
                 var  palestrante = await _redeSocialPersistence.GetAllByPalestranteIdAsync(palestranteId);

                    if (palestrante == null) return null;
                    foreach (var model in models)
                    {
                        model.PalestranteId = palestranteId;

                    if (model.Id == 0)
                    {
                        await AddRedeSocial(0, palestranteId, model);
                    }
                    else
                    {
                        await UpdateRedeSocial(model);
                    }
                }
                var RedeSocialRetorno = await _redeSocialPersistence.GetAllByPalestranteIdAsync(palestranteId);
                return _mapper.Map<RedeSocialDto[]>(RedeSocialRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteRedeSocialByEvento(int eventoId, int RedeSocialId)
        {
            try
            {
                    var RedeSocial = await _redeSocialPersistence.GetRedeSocialEventoByIdAsync(eventoId, RedeSocialId);

                    if (RedeSocial == null) 
                        throw new Exception("A RedeSocial não foi encontrada para deletar!");

                    _redeSocialPersistence.Delete(RedeSocial);


                    return await _redeSocialPersistence.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteRedeSocialByPalestante(int palestranteId, int RedeSocialId)
        {
            try
            {
                var RedeSocial = await _redeSocialPersistence.GetRedeSocialPalestranteByIdAsync(palestranteId, RedeSocialId);

                if (RedeSocial == null)
                    throw new Exception("A RedeSocial não foi encontrada para deletar!");

                _redeSocialPersistence.Delete(RedeSocial);


                return await _redeSocialPersistence.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateRedeSocial(RedeSocialDto model)
        {
            try
            {
                var RedeSocialModel = _mapper.Map<RedeSocial>(model);
                _geralPersistence.Update<RedeSocial>(RedeSocialModel);
                await _geralPersistence.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddRedeSocial(int eventoId , int palestranteId, RedeSocialDto model)
        {
            try
            {
                var RedeSocial = _mapper.Map<RedeSocial>(model);
                RedeSocial.EventoId = (eventoId != 0) ? eventoId : null;
                RedeSocial.PalestranteId = (palestranteId != 0) ? palestranteId : null; 


                _redeSocialPersistence.Add<RedeSocial>(RedeSocial);
                await _redeSocialPersistence.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}