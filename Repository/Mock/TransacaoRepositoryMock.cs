using DTO.Conta;
using DTO.Transacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Mock
{
    /// <summary>
    /// Classe de transação para simular banco 
    /// </summary>
    public class TransacaoRepositoryMock : TransacaoRepository
    {
        private List<TransacaoDTO> listaTransacoes;

        private ContaRepository contaRepository;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="contaRepository">repositorio da conta</param>
        public TransacaoRepositoryMock(ContaRepository contaRepository)
        {
            this.contaRepository = contaRepository;
            listaTransacoes = new List<TransacaoDTO>();
            popularLista();
        }

        /// <summary>
        /// Listar transações de uma conta
        /// </summary>
        /// <param name="conta">objeto conta</param>
        /// <returns>lista de contas</returns>
        public Task<List<TransacaoDTO>> ListarTransacoes(ContaDTO conta)
        {
            return Task.FromResult(listaTransacoes.FindAll(c => c.ContaOrigem.Numero == conta.Numero).ToList());
        }

        /// <summary>
        /// Realizar transação
        /// </summary>
        /// <param name="transacao"></param>
        /// <returns></returns>
        public Task<bool> RealizarTransacaoAsync(TransacaoDTO transacao)
        {
            transacao.ContaOrigem.Saldo = contaRepository.VerificarSaldoAsync(transacao.ContaOrigem).Result - transacao.Valor;
            transacao.ContaDestino.Saldo = contaRepository.VerificarSaldoAsync(transacao.ContaDestino).Result + transacao.Valor;
            transacao.Data = DateTime.Now;

            listaTransacoes.Add(transacao);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Lista de conta "mock"
        /// </summary>
        private void popularLista()
        {
            for (int i = 0; i < 8; i++)
            {
                //conta origem
                var transacao = new TransacaoDTO();
                var conta = new ContaDTO();
                conta.Ativa = true;
                conta.Id = i;
                conta.Numero = $"000{i}";
                conta.Tipo = Tipo.Corrente;
                conta.Saldo = contaRepository.VerificarSaldoAsync(conta).Result;
                transacao.ContaOrigem = conta;

                //criar conta diferente da origem
                var contaDestino = new ContaDTO();
                contaDestino.Ativa = true;
                contaDestino.Id = i + 1;
                contaDestino.Numero = $"000{i + 1}";
                contaDestino.Tipo = Tipo.Corrente;
                contaDestino.Saldo = contaRepository.VerificarSaldoAsync(contaDestino).Result;
                transacao.ContaDestino = contaDestino;

                transacao.Valor = 100;

                listaTransacoes.Add(transacao);
            }
        }
    }
}
