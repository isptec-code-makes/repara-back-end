using AutoMapper;
using DAL.Repositories.Contracts;
using Repara.DTO;
using Repara.DTO.PecaPedido;
using Repara.Model;
using Repara.Services.Contracts;
using Repara.Shared.Exceptions;


namespace Repara.Services
{

    public class PecaPedidoService : IPecaPedidoService
    {
        private readonly IPecaPedidoRepository _pecaPedidoRepository;
        private readonly IMapper _mapper;

        public PecaPedidoService(IPecaPedidoRepository pecaPedidoRepository, IMapper mapper)
        {
            _pecaPedidoRepository = pecaPedidoRepository;
            _mapper = mapper;
        }

        public PagedList<PecaPedidoDTO> GetAllPaged(PecaPedidoFilterParameters parameters)
        {
            var pecaPedidos = _pecaPedidoRepository.GetAllPaged(parameters);
            return _mapper.Map<PagedList<PecaPedidoDTO>>(pecaPedidos);

        }

        public async Task<PecaPedidoDTO?> GetByIdAsync(int id)
        {
            var pecaPedido = await _pecaPedidoRepository.GetByIdAsync(id);
            if (pecaPedido is null) return null;

            return _mapper.Map<PecaPedidoDTO>(pecaPedido);
        }

        public async Task<PecaPedidoDTO?> CreateAsync(PecaPedidoCreateDTO request)
        {
            throw new NotImplementedException();
        }

        public async Task<PecaPedidoDTO?> UpdateAsync(int id, PecaPedidoUpdateDTO request)
        {

            bool changed = false;

            var pecaPedido = await _pecaPedidoRepository.GetByIdAsync(id);
            if (pecaPedido is null) return null;


            if (request.Estado.HasValue && pecaPedido.Estado != request.Estado.Value)
            {
                pecaPedido.Estado = request.Estado.Value;
            }

            _pecaPedidoRepository.Update(pecaPedido);

            if (changed)
            {
                pecaPedido.UpdatedOn = DateTime.Now;
            }

            try
            {
                await _pecaPedidoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao atualizar pecaPedido", e);
            }

            if (changed)
            {
                // do something
            }

            return _mapper.Map<PecaPedidoDTO>(pecaPedido);
        }

        public async Task DeleteAsync(int id)
        {
            var pecaPedido = await _pecaPedidoRepository.GetByIdAsync(id);
            if (pecaPedido is null)
            {
                throw new NotFoundException("PecaPedido não encontrado");
            }

            _pecaPedidoRepository.Remove(pecaPedido);

            try
            {
                await _pecaPedidoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao deletar pecaPedido", e);
            }
        }
    }
}
