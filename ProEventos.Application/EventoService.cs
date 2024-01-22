using AutoMapper;
using Microsoft.Extensions.Logging;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Model;
using ProEventos.Persistence;
using ProEventos.Persistence.Interfaces;
using ProEventos.Persistence.Pagination;
using System;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersistence _geralPersistence;
        private readonly IEventoPersistence _eventosPersistence;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public EventoService(IGeralPersistence geralPersistence, IEventoPersistence eventosPersistence, IAccountService accountService, IMapper mapper)
        {
            _geralPersistence = geralPersistence;
            _accountService = accountService;
            _eventosPersistence = eventosPersistence;
            _mapper = mapper;
        }
        public async Task<EventoDto> AddEvento(int userId, EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;

                _geralPersistence.Add<Evento>(evento);
                if (await _geralPersistence.SaveChangesAsync())
                {
                    var eventos =  await _eventosPersistence.GetEventoByIdAsync(userId, evento.Id, false);

                    return _mapper.Map<EventoDto>(eventos);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int userId, int id, EventoDto model)
        {
            try
            {
                var evento = await _eventosPersistence.GetEventoByIdAsync(userId, id, false);
                if (evento == null) return null;


                _mapper.Map(model, evento);

                _geralPersistence.Update<Evento>(evento);

                if (await _geralPersistence.SaveChangesAsync())
                {
                    var eventoRetorno =  await _eventosPersistence.GetEventoByIdAsync(evento.UserId, evento.Id, false);
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int userId, int id)
        {
            try
            {
                var evento = await _eventosPersistence.GetEventoByIdAsync(userId, id, false);

                if (evento == null) throw new Exception("O Evento não foi encontrado para deletar!");

                _geralPersistence.Delete(evento);
                return await _geralPersistence.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes)
        {
            try
            {
                var eventos = await _eventosPersistence.GetAllEventosAsync(userId, pageParams, includePalestrantes);
                if (eventos == null) return null;


                var resultado = _mapper.Map<PageList<EventoDto>>(eventos);

                resultado.CurrentPage = eventos.CurrentPage;
                resultado.TotalPages = eventos.TotalPages;
                resultado.PageSize = eventos.PageSize;
                resultado.TotalCount = eventos.TotalCount;


                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<EventoDto> GetEventoByIdAsync(int userId, int id, bool includePalestrantes)
        {
            try
            {
                var eventos = await _eventosPersistence.GetEventoByIdAsync(userId, id, includePalestrantes);
                if (eventos == null) return null;

                return _mapper.Map<EventoDto>(eventos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}