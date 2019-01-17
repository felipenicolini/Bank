using DTO.Conta;
using DTO.Transacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// Interface
    /// </summary>
    public interface TransacaoRepository
    {
        Task<List<TransacaoDTO>> ListarTransacoes(ContaDTO conta);
        Task<bool> RealizarTransacaoAsync(TransacaoDTO transacao);
    }
}
