# Bank
Create Transaction Bank .Net Core

Exemplos de chamadas:

[POST] - Efetua uma transação entre as contas atualizando os valores
http://localhost:5000/api/transacao

HEADER
Content-Type application/json
Accept application/json

{
	"contaOrigem": 
  	{
    	"numero": "0001",
    },
	"contaDestino": 
  	{
		"numero": "0002",
	},
	"valor": 100
}


[GET] - Busca todas transções da conta
http://localhost:5000/api/transacao/listar/{NUMERO DA CONTA}
ex: http://localhost:5000/api/transacao/listar/0001
