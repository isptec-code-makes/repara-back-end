using Repara.Model.Enum;

namespace Repara.Model;

// TODO: Adicionar os data anotations
// Esta classe abstrai um serviço que é prestado pela empresa
public abstract class Servico : TableBase
{
    // atributo para armazenar o estado do serviço
    private ServicoEstado _estado = ServicoEstado.Pendente;

    public string Especialidade { get; set; } = string.Empty;

    // Data de inicialização do serviçço
    public DateTime? DateInit { get; set; } = null;

    // Data de finalização do serviço
    public DateTime? DateEnd { get; set; } = null;

    // Relatorio do Serviço criado pelo funcionario
    public string? Relatorio { get; set; } = string.Empty;

    // Id do funcionario
    public int? FuncionarioId { get; set; }

    // Funcionario Responsável pelo serviço
    public Funcionario? Funcionario { get; set; }

    // TODO: Não mapear na BD
    public ServicoEstado Estado
    {
        get => _estado;
        set
        {
            // quando o estado é mudado para Terminado, seta a data de fim de serviço com a data e hora actual
            if (value is ServicoEstado.Terminado)
            {
                DateEnd = DateTime.Now;
            }

            // quando o estado é mudado para iniciado, seta a data de inicio de serviço com a data e hora actual
            if (value is ServicoEstado.Iniciado)
            {
                DateInit = DateTime.Now;
            }

            // quando o estado é mudado para cancelado, seta a data de fim de serviço com a data e hora actual
            if (value is ServicoEstado.Cancelado)
            {
                DateEnd = DateTime.Now;
            }

            _estado = value;

        }
    }
}