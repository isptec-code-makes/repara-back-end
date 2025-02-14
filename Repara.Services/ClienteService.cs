using AutoMapper;
using DAL.Repositories.Contracts;
using Repara.DTO;
using Repara.DTO.Cliente;
using Repara.Model;
using Repara.Services.Contracts;
using Repara.Shared.Exceptions;


namespace Repara.Services
{

    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public PagedList<ClienteDTO> GetAllPaged(ClienteFilterParameters parameters)
        {
            var clientes = _clientRepository.GetAllPaged(parameters);
            return _mapper.Map<PagedList<ClienteDTO>>(clientes);

        }

        public async Task<ClienteDTO?> GetByIdAsync(int id)
        {
            var cliente = await _clientRepository.GetByIdAsync(id);
            if (cliente is null) return null;

            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<ClienteDTO?> CreateAsync(ClienteCreateDTO request)
        {
            // var exists = await _clientRepository.AnyAsync(c=> (c.Email != null && c.Email.Equals(request.Email)) || c.Telefone.Equals(request.Telefone));

            // if (exists) return null;

            var cliente = _mapper.Map<Cliente>(request);

            _clientRepository.Add(cliente);

            try
            {
                await _clientRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao salvar cliente", e);
            }

            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<ClienteDTO?> UpdateAsync(int id, ClienteUpdateDTO request)
        {
            var cliente = await _clientRepository.GetByIdAsync(id);
            if (cliente is null) return null;

            _mapper.Map(request, cliente);

            _clientRepository.Update(cliente);

            try
            {
                await _clientRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao atualizar cliente", e);
            }

            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await _clientRepository.GetByIdAsync(id);
            if (cliente is null)
            {
                throw new NotFoundException("Cliente não encontrado");
            }

            _clientRepository.Remove(cliente);

            try
            {
                await _clientRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao deletar cliente", e);
            }
        }
    }
}
