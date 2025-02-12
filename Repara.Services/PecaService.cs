using AutoMapper;
using DAL.Repositories.Contracts;
using Repara.DTO;
using Repara.DTO.Peca;
using Repara.Model;
using Repara.Services.Contracts;
using Repara.Shared.Exceptions;


namespace Repara.Services
{

    public class PecaService : IPecaService
    {
        private readonly IPecaRepository _pecaRepository;
        private readonly IMapper _mapper;

        public PecaService(IPecaRepository pecaRepository, IMapper mapper)
        {
            _pecaRepository = pecaRepository;
            _mapper = mapper;
        }

        public PagedList<PecaDTO> GetAllPaged(PecaFilterParameters parameters)
        {
            var pecas = _pecaRepository.GetAllPaged(parameters);
            return _mapper.Map<PagedList<PecaDTO>>(pecas);

        }

        public async Task<PecaDTO?> GetByIdAsync(int id)
        {
            var peca = await _pecaRepository.GetByIdAsync(id);
            if (peca is null) return null;

            return _mapper.Map<PecaDTO>(peca);
        }

        public async Task<PecaDTO?> CreateAsync(PecaCreateDTO request)
        {
            var peca = _mapper.Map<Peca>(request);

            _pecaRepository.Add(peca);

            try
            {
                await _pecaRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao salvar peca", e);
            }

            return _mapper.Map<PecaDTO>(peca);
        }

        public async Task<PecaDTO?> UpdateAsync(int id, PecaUpdateDTO request)
        {
            var peca = await _pecaRepository.GetByIdAsync(id);
            if (peca is null) return null;

            _mapper.Map(request, peca);

            _pecaRepository.Update(peca);

            try
            {
                await _pecaRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao atualizar peca", e);
            }

            return _mapper.Map<PecaDTO>(peca);
        }

        public async Task DeleteAsync(int id)
        {
            var peca = await _pecaRepository.GetByIdAsync(id);
            if (peca is null)
            {
                throw new NotFoundException("Peca não encontrado");
            }

            _pecaRepository.Remove(peca);

            try
            {
                await _pecaRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao deletar peca", e);
            }
        }
    }
}
