namespace DTO.Conta
{
    /// <summary>
    /// Dto da Conta
    /// </summary>
    public class ContaDTO
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public Tipo Tipo { get; set; }
        public bool Ativa { get; set; }
        public decimal Saldo { get; set; }
    }
}
