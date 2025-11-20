using Microsoft.AspNetCore.Mvc;
using Parcial2DDA.RequestDtos;
using Parcial2DDA.ResponseDtos;
using Parcial2DDA.Services.Interfaces;

namespace Parcial2DDA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PesoController : ControllerBase
    {
        private readonly IPesoService _pesoService;

        public PesoController(IPesoService pesoService)
        {
            _pesoService = pesoService;
        }

        [HttpPost("/medicion")]
        public async Task<ActionResult> Medicion([FromBody] PesoRequestDto pesoDto)
        {
            if (string.IsNullOrEmpty(pesoDto.Huella) || string.IsNullOrEmpty(pesoDto.TipoEntrada) || pesoDto.Peso <= 0)
            {
                return BadRequest("Faltan datos obligatorios.");
            }

            if(pesoDto.TipoEntrada.ToLower() == "entrada")
            {
                string resultadoIngreso = await _pesoService.IngresarPeso(pesoDto);
                return Ok(resultadoIngreso);
            }
            else if(pesoDto.TipoEntrada.ToLower() == "salida")
            {
                PesoResponseDto resultadoMedicion = await _pesoService.MedirPeso(pesoDto, DateTime.Now);
                if (resultadoMedicion == null)
                {
                    return NotFound("No se encontró un registro de entrada para la huella proporcionada en la fecha actual.");
                }
                return Ok(resultadoMedicion);
            } else
            {
                return BadRequest("Tipo de entrada no válido. Use 'entrada' o 'salida'.");
            }
        }

        [HttpGet("/reportes/total")]
        public async Task<ActionResult> TotalReportes()
        {
            int total = await _pesoService.TotalReportes();
            return Ok(new { total_mediciones_completadas = total });
        }

        [HttpGet("/reportes/maxima-diferencia-peso")]
        public async Task<ActionResult> MaximaDiferenciaPeso()
        {
            decimal maximoPeso = await _pesoService.PesoMaximo();
            return Ok(new { maxima_diferencia_peso = maximoPeso });
        }

        [HttpGet("/reportes/maximo-tiempo")]
        public ActionResult MaximoTiempo()
        {
            string maximoTiempo = _pesoService.MaximoTiempoEnLocal();
            return Ok(new { maximo_tiempo = maximoTiempo });
        }
    }
}
