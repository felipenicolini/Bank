using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO.Transacao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Factory;

namespace Api.Controllers
{
    /// <summary>
    /// Classe controller transação
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransacaoController : ControllerBase
    {
        private TransacaoRepository transacaoRepository;
        private ContaRepository contaRepository;

        /// <summary>
        /// Contrutor default, recebendo DI os repositorios
        /// </summary>
        /// <param name="transacaoRepository">Repositorio de transação</param>
        /// <param name="contaRepository">Repositório de conta</param>
        public TransacaoController(TransacaoRepository transacaoRepository, ContaRepository contaRepository)
        {
            this.transacaoRepository = transacaoRepository;
            this.contaRepository = contaRepository;
        }

        /// <summary>
        /// Serviço para listar transações da conta
        /// </summary>
        /// <param name="numeroConta">numero da conta</param>
        /// <returns>Dados das transações</returns>
        [HttpGet("Listar/{numeroConta}")]
        public async Task<IActionResult> ListarAsync(string numeroConta)
        {
            var objetoContaValido = ContaFactory.CriarObjetoContaValido(numeroConta);
            var conta = await contaRepository.VerificarContaExisteAtivaAsync(objetoContaValido);

            if (conta.Id != 0)
            {
                var transacoes = await transacaoRepository.ListarTransacoes(conta);

                if (transacoes != null)
                {
                    return Ok(transacoes);
                }
            }

            return NotFound("Conta inexistente");
        }

        /// <summary>
        /// Serviço para efetuar transação, creditar e debitar das contas
        /// </summary>
        /// <param name="transacao">Transação com contas e valor</param>
        /// <returns>Status da transação</returns>
        [HttpPost]
        public async Task<IActionResult> CriarTransacaoAsync([FromBody] TransacaoDTO transacao)
        {
            if (transacao == null)
            {
                return BadRequest("Transação nula");
            }
            if (transacao.ContaDestino == null)
            {
                return BadRequest("Conta destino nula");
            }
            if (transacao.ContaOrigem == null)
            {
                return BadRequest("Conta origem nula");
            }
            if (transacao.Valor <= 0)
            {
                return BadRequest("Valor da transação inválido");
            }

            var contaOrigem = await contaRepository.VerificarContaExisteAtivaAsync(transacao.ContaOrigem);
            var contaDestino = await contaRepository.VerificarContaExisteAtivaAsync(transacao.ContaDestino);

            if (contaOrigem.Id == 0 || contaDestino.Id == 0)
            {
                return BadRequest("Conta inválida");
            }

            var saldoContaOrigem = await contaRepository.VerificarSaldoAsync(contaOrigem);

            if (contaOrigem.Numero == transacao.ContaDestino.Numero)
            {
                return BadRequest("Conta destino deve ser diferente da origem");
            }

            if (transacao.Valor > saldoContaOrigem)
            {
                return BadRequest("Valor excede o total da conta");
            }

            var realizouTransacao = await transacaoRepository.RealizarTransacaoAsync(transacao);

            if (realizouTransacao)
            {
                await contaRepository.DebitarValorAsync(contaOrigem, transacao.Valor);
                await contaRepository.CreditarValorAsync(contaDestino, transacao.Valor);

                return Ok("Transação realizada com sucesso");
            }

            return BadRequest("Não foi possível realizar transação, verifique se as contas estão corretas!");
        }
    }
}
