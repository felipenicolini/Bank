using DTO.Conta;
using System;

namespace DTO.Transacao
{
    /// <summary>
    /// DTO da transacao
    /// </summary>
    public class TransacaoDTO
    {
        public ContaDTO ContaOrigem { get; set; }
        public ContaDTO ContaDestino { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }
}
