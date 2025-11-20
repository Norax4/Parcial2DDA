using Parcial2DDA.RequestDtos;
using Parcial2DDA.ResponseDtos;

namespace Parcial2DDA.Services.Interfaces
{
    public interface IPesoService
    {
        Task<string> IngresarPeso(PesoRequestDto pesoDto);
        Task<PesoResponseDto> MedirPeso(PesoRequestDto pesoDto, DateTime fecha);
        string CalcularTiempo(DateTime fecha1, DateTime fecha2);
        Task<int> TotalReportes();
        Task<decimal> PesoMaximo();
        string MaximoTiempoEnLocal();
    }
}
