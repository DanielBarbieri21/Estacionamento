using System;

namespace Estacionamento
{
    public class Veiculo
    {
        public required string Placa { get; set; } 
        public required string Tipo { get; set; } 
        public DateTime Entrada { get; set; }
        public DateTime? Saida { get; set; }
        public double ValorHora { get; set; }

        public TimeSpan TempoPermanencia => (Saida ?? DateTime.Now) - Entrada;

        public double CalcularValor()
        {
            var horas = Math.Ceiling(TempoPermanencia.TotalHours);
            return horas * ValorHora;
        }
    }
}
