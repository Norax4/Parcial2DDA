namespace Parcial2DDA.Models
{
    public class PesoCliente
    {
        public int Id { get; set; }
        public string Huella { get; set; }
        public decimal Peso { get; set; }
        public string TipoEntrada { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
