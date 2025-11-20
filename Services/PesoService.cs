using Microsoft.EntityFrameworkCore;
using Parcial2DDA.Data;
using Parcial2DDA.Models;
using Parcial2DDA.RequestDtos;
using Parcial2DDA.ResponseDtos;
using Parcial2DDA.Services.Interfaces;

namespace Parcial2DDA.Services
{
    public class PesoService : IPesoService
    {
        private readonly AppDbContext _context;


        public PesoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> IngresarPeso(PesoRequestDto pesoDto)
        {
            PesoCliente pesoCliente = new PesoCliente
            {
                Huella = pesoDto.Huella,
                Peso = pesoDto.Peso,
                TipoEntrada = pesoDto.TipoEntrada.ToLower(),
                FechaRegistro = DateTime.Now
            };

            _context.PesosClientes.Add(pesoCliente);
            await _context.SaveChangesAsync();
            return "Peso ingresado correctamente.";
        }

        public async Task<PesoResponseDto> MedirPeso(PesoRequestDto pesoDto, DateTime fecha)
        {
            PesoCliente? pesoClienteEntrada = await _context.PesosClientes
                .FirstOrDefaultAsync(p => p.Huella == pesoDto.Huella && p.TipoEntrada == "entrada" && 
                p.FechaRegistro.Day == fecha.Day && p.FechaRegistro.Month == fecha.Month);
            
            if (pesoClienteEntrada == null)
            {
                return null;
            }

            string TiempoEnLocal = CalcularTiempo(pesoClienteEntrada.FechaRegistro, fecha);

            ReporteMedicion reporte = new ReporteMedicion
            {
                Huella = pesoDto.Huella,
                DiferenciaPeso = pesoDto.Peso - pesoClienteEntrada.Peso,
                FechaRegistro = pesoClienteEntrada.FechaRegistro,
                FechaSalida = fecha,
                TiempoEnLocal = TiempoEnLocal
            };

            _context.ReportesMediciones.Add(reporte);
            _context.PesosClientes.Remove(pesoClienteEntrada);
            await _context.SaveChangesAsync();

            PesoResponseDto response = new PesoResponseDto
            {
                Huella = reporte.Huella,
                DiferenciaPeso = reporte.DiferenciaPeso,
                TiempoEnLocal = reporte.TiempoEnLocal
            };

            return response;
        }

        public async Task<int> TotalReportes()
        {
            return await _context.ReportesMediciones.CountAsync();
        }

        public async Task<decimal> PesoMaximo()
        { 
            ReporteMedicion reporteMaximo = await _context.ReportesMediciones
                .OrderByDescending(r => r.DiferenciaPeso)
                .FirstOrDefaultAsync();

            if (reporteMaximo == null)
            {
                return 0;
            }

            return reporteMaximo.DiferenciaPeso;
        }

        public string MaximoTiempoEnLocal()
        {
            ReporteMedicion? reporteMaximoTiempo = _context.ReportesMediciones
                .AsEnumerable()
                .OrderByDescending(r => 
                {
                    var tiempoPartes = r.TiempoEnLocal.Split(new string[] { " horas, ", " minutos" }, StringSplitOptions.RemoveEmptyEntries);
                    int horas = int.Parse(tiempoPartes[0]);
                    int minutos = int.Parse(tiempoPartes[1]);
                    return horas * 60 + minutos;
                })
                .FirstOrDefault();

            if (reporteMaximoTiempo == null)
            {
                return "0 horas, 0 minutos";
            }
            return reporteMaximoTiempo.TiempoEnLocal;
        }

        public string CalcularTiempo(DateTime fecha1, DateTime fecha2)
        {
            long totalSegundos1 = new DateTimeOffset(fecha1).ToUnixTimeSeconds();
            long totalSegundos2 = new DateTimeOffset(fecha2).ToUnixTimeSeconds();

            int diferenciaSegundos = (int)(totalSegundos2 - totalSegundos1);
            Console.WriteLine($"Diferencia en segundos: {diferenciaSegundos}");

            int horas = diferenciaSegundos / 3600;
            int minutos = (diferenciaSegundos % 3600) / 60;
            string tiempoTranscurrido = $"{horas} horas, {minutos} minutos";

            return tiempoTranscurrido;
        }
    }
}
