namespace Repara.Model.Enum;

public enum SolicitacaoEstado
{
    /*
    Recebido = 1,
    Diagnostico,
    Montagem,
    Terminado,
    */
    Pendente = 1, // a solicitação foi recebida e está pendente de atendimento
    Andamento, // a solicitação está sendo atendida
    Concluido, // a solicitação foi concluída, todos os equipamentos foram atendidos
    Entregue // Moi entrege ao cliente
}