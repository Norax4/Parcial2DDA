namespace Parcial2DDA.Models
{
    public class ReporteMedicion
    {
        public int Id { get; set; }
        public string Huella { get; set; }
        public decimal DiferenciaPeso { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaSalida { get; set; }
        public string TiempoEnLocal { get; set; }
    }
}
