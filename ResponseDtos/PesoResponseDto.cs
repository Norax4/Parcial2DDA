using System.Text.Json.Serialization;

namespace Parcial2DDA.ResponseDtos
{
    public class PesoResponseDto
    {
        [JsonPropertyName("huella")]
        public string Huella { get; set; }
        [JsonPropertyName("diferenciaPeso")]
        public decimal DiferenciaPeso { get; set; }
        [JsonPropertyName("tiempoEnLocal")]
        public string TiempoEnLocal { get; set; }
    }
}
