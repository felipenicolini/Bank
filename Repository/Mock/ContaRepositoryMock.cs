using DTO.Conta;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Mock
{
    /// <summary>
    /// Repositório para simular banco de dados
    /// </summary>
    public class ContaRepositoryMock : ContaRepository
    {
        private List<ContaDTO> listaContas;

        /// <summary>
        /// Contrutor padrão
        /// </summary>
        public ContaRepositoryMock()
        {
            listaContas = new List<ContaDTO>();
            popularLista();
        }

        /// <summary>
        /// Creditar valor de uma conta
        /// </summary>
        /// <param name="conta">Objeto conta</param>
        /// <param name="valor">Valor para ser creditado</param>
        /// <returns>retorna o valor após creditar</returns>
        public Task<decimal> CreditarValorAsync(ContaDTO conta, decimal valor)
        {
            var contaCreditar = listaContas.FindAll(c => c.Numero == conta.Numero).FirstOrDefault();
            contaCreditar.Saldo = contaCreditar.Saldo + valor;
            return Task.FromResult(contaCreditar.Saldo);
        }

        /// <summary>
        /// Debitar valor de uma conta
        /// </summary>
        /// <param name="conta">Objeto conta</param>
        /// <param name="valor">Valor para ser debitado</param>
        /// <returns>retorna o valor após debitar</returns>
        public Task<decimal> DebitarValorAsync(ContaDTO conta, decimal valor)
        {
            var contaDebitar = listaContas.FindAll(c => c.Numero == conta.Numero).FirstOrDefault();
            contaDebitar.Saldo = contaDebitar.Saldo - valor;
            return Task.FromResult(contaDebitar.Saldo);
        }

        /// <summary>
        /// Lista de contas
        /// </summary>
        /// <returns>retorna uma lista</returns>
        public Task<List<ContaDTO>> ListarContasAsync()
        {
            return Task.FromResult(this.listaContas);
        }

        /// <summary>
        /// Verificar se a conta existe e está ativa
        /// </summary>
        /// <param name="conta">Objeto conta</param>
        /// <returns>retorna conta</returns>
        public Task<ContaDTO> VerificarContaExisteAtivaAsync(ContaDTO conta)
        {
            var contaExiste = listaContas.FindAll(c => c.Numero == conta.Numero && c.Ativa == true);

            if (contaExiste.Count > 0)
            {
                return Task.FromResult(contaExiste.FirstOrDefault());
            }

            return Task.FromResult(new ContaDTO());
        }

        /// <summary>
        /// Verificar saldo de uma determinada conta
        /// </summary>
        /// <param name="conta">Buscando pelo numero da conta</param>
        /// <returns>retorna saldo atual da conta</returns>
        public Task<decimal> VerificarSaldoAsync(ContaDTO conta)
        {
            var contaExiste = listaContas.FindAll(c => c.Numero == conta.Numero && c.Ativa == true);

            if (contaExiste.Count > 0)
            {
                return Task.FromResult(contaExiste.FirstOrDefault().Saldo);
            }

            return Task.FromResult(0m);
        }

        /// <summary>
        /// Lista de conta "mock"
        /// </summary>
        private void popularLista()
        {
            for (int i = 0; i < 8; i++)
            {
                var conta = new ContaDTO();
                conta.Ativa = true;
                conta.Id = i;
                conta.Numero = $"000{i}";
                conta.Tipo = Tipo.Corrente;
                conta.Saldo = 100.00m;

                listaContas.Add(conta);
            }
        }
    }
}
