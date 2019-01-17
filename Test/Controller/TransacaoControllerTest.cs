using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Test.Mock;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using DTO.Transacao;
using DTO.Conta;

namespace Test.Controller
{
    public class TransacaoControllerTest
    {
        [Fact(DisplayName = "Criar Transacao Nula")]
        public async Task CriarTransacaoNula()
        {
            var transacaoRepository = new TransacaoRepositoryMock();
            var contaRepository = new ContaRepositoryMock();

            var controller = new TransacaoController(transacaoRepository, contaRepository);

            var createResult = await controller.CriarTransacaoAsync(null);

            Assert.IsType<BadRequestObjectResult>(createResult);

            var badRequestResult = createResult as BadRequestObjectResult;

            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Transação nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "Transacao Conta Origem Nula")]
        public async Task TransacaoContaOrigemNula()
        {
            var transacaoRepository = new TransacaoRepositoryMock();
            var contaRepository = new ContaRepositoryMock();

            var controller = new TransacaoController(transacaoRepository, contaRepository);

            var transacao = new TransacaoDTO();
            transacao.ContaOrigem = null;

            var contaDestino = new ContaDTO();
            contaDestino.Ativa = true;
            contaDestino.Numero = "0002";
            transacao.ContaDestino = contaDestino;
            transacao.Valor = 40.00m;

            var createResult = await controller.CriarTransacaoAsync(transacao);

            Assert.IsType<BadRequestObjectResult>(createResult);

            var badRequestResult = createResult as BadRequestObjectResult;

            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Conta origem nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "Transacao Conta Destino Nula")]
        public async Task TransacaoContaDestinoNula()
        {
            var transacaoRepository = new TransacaoRepositoryMock();
            var contaRepository = new ContaRepositoryMock();

            var controller = new TransacaoController(transacaoRepository, contaRepository);

            var transacao = new TransacaoDTO();
            var ContaOrigem = new ContaDTO();
            ContaOrigem.Ativa = true;
            ContaOrigem.Numero = "0002";
            transacao.ContaDestino = null;
            transacao.ContaOrigem = ContaOrigem;

            transacao.Valor = 40.00m;

            var createResult = await controller.CriarTransacaoAsync(transacao);

            Assert.IsType<BadRequestObjectResult>(createResult);

            var badRequestResult = createResult as BadRequestObjectResult;

            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Conta destino nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "Transacao Conta Repetida")]
        public async Task TransacaoComMesmaConta()
        {
            var transacaoRepository = new TransacaoRepositoryMock();
            var contaRepository = new ContaRepositoryMock();

            var controller = new TransacaoController(transacaoRepository, contaRepository);

            var transacao = new TransacaoDTO();
            var ContaOrigem = new ContaDTO();
            ContaOrigem.Ativa = true;
            ContaOrigem.Numero = "0002";
            transacao.ContaDestino = ContaOrigem;
            transacao.ContaOrigem = ContaOrigem;

            transacao.Valor = 40.00m;

            var createResult = await controller.CriarTransacaoAsync(transacao);

            Assert.IsType<BadRequestObjectResult>(createResult);

            var badRequestResult = createResult as BadRequestObjectResult;

            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Conta destino deve ser diferente da origem", badRequestResult.Value);
        }

        [Fact(DisplayName = "Transacao Conta Inexistente")]
        public async Task TransacaoContaInexistente()
        {
            var transacaoRepository = new TransacaoRepositoryMock();
            var contaRepository = new ContaRepositoryMock();

            var controller = new TransacaoController(transacaoRepository, contaRepository);

            var transacao = new TransacaoDTO();
            var ContaOrigem = new ContaDTO();
            ContaOrigem.Ativa = true;
            ContaOrigem.Numero = "0003";// só existe 0001 e 0002
            transacao.ContaOrigem = ContaOrigem;

            var ContaDestino = new ContaDTO();
            ContaDestino.Ativa = true;
            ContaDestino.Numero = "0004";// só existe 0001 e 0002
            transacao.ContaDestino = ContaDestino;

            transacao.Valor = 40.00m;

            var createResult = await controller.CriarTransacaoAsync(transacao);

            Assert.IsType<BadRequestObjectResult>(createResult);

            var badRequestResult = createResult as BadRequestObjectResult;

            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Conta inválida", badRequestResult.Value);
        }

        [Fact(DisplayName = "Transação excede valor")]
        public async Task TransacaoValorExcedido()
        {
            var transacaoRepository = new TransacaoRepositoryMock();
            var contaRepository = new ContaRepositoryMock();

            var controller = new TransacaoController(transacaoRepository, contaRepository);

            var transacao = new TransacaoDTO();
            var ContaOrigem = new ContaDTO();
            ContaOrigem.Ativa = true;
            ContaOrigem.Numero = "0001";
            transacao.ContaOrigem = ContaOrigem;

            var ContaDestino = new ContaDTO();
            ContaDestino.Ativa = true;
            ContaDestino.Numero = "0002";
            transacao.ContaDestino = ContaDestino;

            transacao.Valor = 200.00m;

            var createResult = await controller.CriarTransacaoAsync(transacao);

            Assert.IsType<BadRequestObjectResult>(createResult);

            var badRequestResult = createResult as BadRequestObjectResult;

            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Valor excede o total da conta", badRequestResult.Value);
        }


        [Fact(DisplayName = "Transação efetuada com sucesso")]
        public async Task TransacaoEfetuadaComSUcesso()
        {
            var transacaoRepository = new TransacaoRepositoryMock();
            var contaRepository = new ContaRepositoryMock();

            var controller = new TransacaoController(transacaoRepository, contaRepository);

            var transacao = new TransacaoDTO();
            var ContaOrigem = new ContaDTO();
            ContaOrigem.Ativa = true;
            ContaOrigem.Numero = "0001";
            transacao.ContaOrigem = ContaOrigem;

            var ContaDestino = new ContaDTO();
            ContaDestino.Ativa = true;
            ContaDestino.Numero = "0002";
            transacao.ContaDestino = ContaDestino;

            transacao.Valor = 10.0m;

            var createResult = await controller.CriarTransacaoAsync(transacao);

            Assert.IsType<OkObjectResult>(createResult);
            var okResult = createResult as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);
        }

        

        
    }
}
