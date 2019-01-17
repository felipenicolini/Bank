using DTO.Conta;

namespace Factory
{
    /// <summary>
    /// Classe factory
    /// </summary>
    public static class ContaFactory
    {
        /// <summary>
        /// Criar um objeto válido para entidade conta
        /// </summary>
        /// <param name="numeroConta">numero da conta</param>
        /// <returns>retorna com um numero da conta válido</returns>
        public static ContaDTO CriarObjetoContaValido(string numeroConta)
        {
            var conta = new ContaDTO();

            if (string.IsNullOrEmpty(numeroConta))
            {
                conta.Numero = "0";
            }
            else
            {
                conta.Numero = numeroConta;
            }
            return conta;
        }
    }
}
