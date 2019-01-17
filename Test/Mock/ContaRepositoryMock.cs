using DTO.Conta;
using Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Mock
{
    public class ContaRepositoryMock : ContaRepository
    {
        private List<ContaDTO> listaContas;

        public ContaRepositoryMock()
        {
            listaContas = new List<ContaDTO>();
            popularLista();
        }

        public Task<decimal> CreditarValorAsync(ContaDTO conta, decimal valor)
        {
            var contaCreditar = listaContas.FindAll(c => c.Numero == conta.Numero).FirstOrDefault();
            contaCreditar.Saldo = contaCreditar.Saldo + valor;
            return Task.FromResult(contaCreditar.Saldo);
        }

        public Task<decimal> DebitarValorAsync(ContaDTO conta, decimal valor)
        {
            var contaDebitar = listaContas.FindAll(c => c.Numero == conta.Numero).FirstOrDefault();
            contaDebitar.Saldo = contaDebitar.Saldo - valor;
            return Task.FromResult(contaDebitar.Saldo);
        }

        public Task<List<ContaDTO>> ListarContasAsync()
        {
            return Task.FromResult(this.listaContas);
        }

        public Task<ContaDTO> VerificarContaExisteAtivaAsync(ContaDTO conta)
        {
            var contaExiste = listaContas.FindAll(c => c.Numero == conta.Numero);

            if (contaExiste.Count > 0)
            {
                return Task.FromResult(contaExiste.FirstOrDefault());
            }

            return Task.FromResult(new ContaDTO());
        }

        public Task<decimal> VerificarSaldoAsync(ContaDTO conta)
        {
            var contaExiste = listaContas.FindAll(c => c.Numero == conta.Numero && c.Ativa == true);

            if (contaExiste.Count > 0)
            {
                return Task.FromResult(contaExiste.FirstOrDefault().Saldo);
            }

            return Task.FromResult(0m);
        }

        private void popularLista()
        {
            var conta = new ContaDTO();
            conta.Ativa = true;
            conta.Id = 1;
            conta.Numero = "0001";
            conta.Tipo = Tipo.Corrente;
            conta.Saldo = 100;

            var contaNova = new ContaDTO();
            contaNova.Ativa = true;
            contaNova.Id = 2;
            contaNova.Numero = "0002";
            contaNova.Tipo = Tipo.Corrente;
            contaNova.Saldo = 200;

            listaContas.Add(conta);
            listaContas.Add(contaNova);
        }
    }
}
