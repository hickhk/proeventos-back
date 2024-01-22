using AutoMapper;
using Microsoft.Extensions.Logging;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Model;
using ProEventos.Persistence;
using ProEventos.Persistence.Interfaces;
using ProEventos.Persistence.Pagination;
using System;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestrantePersistence _PalestrantesPersistence;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public PalestranteService(IGeralPersistence geralPersistence, IPalestrantePersistence PalestrantesPersistence, IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _PalestrantesPersistence = PalestrantesPersistence;
            _mapper = mapper;
        }
        public async Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto model)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;

                _PalestrantesPersistence.Add<Palestrante>(palestrante);
                if (await _PalestrantesPersistence.SaveChangesAsync())
                {
                    var Palestrantes =  await _PalestrantesPersistence.GetPalestranteByUserIdAsync(userId, false);

                    return _mapper.Map<PalestranteDto>(Palestrantes);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model)
        {
            try
            {

                var palestrante = _mapper.Map<Palestrante>(model);

                _PalestrantesPersistence.Update<Palestrante>(palestrante);

                if (await _PalestrantesPersistence.SaveChangesAsync())
                {
                    var PalestranteRetorno =  await _PalestrantesPersistence.GetPalestranteByUserIdAsync(palestrante.UserId,  false);
                    return _mapper.Map<PalestranteDto>(PalestranteRetorno);
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            try
            {
                var Palestrantes = await _PalestrantesPersistence.GetAllPalestrantesAsync(pageParams, includeEventos);
                if (Palestrantes == null) return null;


                var resultado = _mapper.Map<PageList<PalestranteDto>>(Palestrantes);

                resultado.CurrentPage = Palestrantes.CurrentPage;
                resultado.TotalPages = Palestrantes.TotalPages;
                resultado.PageSize = Palestrantes.PageSize;
                resultado.TotalCount = Palestrantes.TotalCount;


                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos )
        {
            try
            {
                var Palestrantes = await _PalestrantesPersistence.GetPalestranteByUserIdAsync(userId, includeEventos);
                if (Palestrantes == null) return null;

                return _mapper.Map<PalestranteDto>(Palestrantes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}