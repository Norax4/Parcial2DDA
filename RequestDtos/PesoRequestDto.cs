using System.Text.Json.Serialization;

namespace Parcial2DDA.RequestDtos
{
    public class PesoRequestDto
    {
        [JsonPropertyName("huella")]
        public string Huella { get; set; }
        [JsonPropertyName("peso")]
        public decimal Peso { get; set; }
        [JsonPropertyName("tipo")]
        public string TipoEntrada { get; set; }
    }
}
