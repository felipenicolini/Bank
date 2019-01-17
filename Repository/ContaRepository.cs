using DTO.Conta;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// Interface
    /// </summary>
    public interface ContaRepository
    {
        Task<decimal> CreditarValorAsync(ContaDTO conta, decimal valor);
        Task<decimal> DebitarValorAsync(ContaDTO conta, decimal valor);
        Task<decimal> VerificarSaldoAsync(ContaDTO conta);
        Task<ContaDTO> VerificarContaExisteAtivaAsync(ContaDTO conta);
        Task<List<ContaDTO>> ListarContasAsync();
    }
}
