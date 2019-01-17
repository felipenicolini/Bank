using DTO.Conta;
using DTO.Transacao;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Test.Mock
{
    public class TransacaoRepositoryMock : TransacaoRepository
    {
        private List<TransacaoDTO> listaTransacoes;

        public TransacaoRepositoryMock()
        {
            listaTransacoes = new List<TransacaoDTO>();
            popularLista();
        }

        public Task<List<TransacaoDTO>> ListarTransacoes(ContaDTO conta)
        {
            return Task.FromResult(listaTransacoes.FindAll(c => c.ContaOrigem.Numero == conta.Numero).ToList());
        }

        public Task<bool> RealizarTransacaoAsync(TransacaoDTO transacao)
        {
            listaTransacoes.Add(transacao);
            return Task.FromResult(true);
        }

        private void popularLista()
        {
            //conta origem
            var transacao = new TransacaoDTO();
            var conta = new ContaDTO();
            conta.Ativa = true;
            conta.Id = 1000;
            conta.Numero = "0001";
            conta.Tipo = Tipo.Corrente;
            transacao.ContaOrigem = conta;

            //conta destino
            var contaDestino = new ContaDTO();
            contaDestino.Ativa = true;
            contaDestino.Id = 2000;
            contaDestino.Numero = "0002";
            contaDestino.Tipo = Tipo.Corrente;
            transacao.ContaDestino = contaDestino;

            transacao.Valor = 100;

            listaTransacoes.Add(transacao);
        }
    }
}
