using System;

namespace Estacionamento.Models
{
    public class Veiculo
    {
        public string Placa { get; set; } = string.Empty;
        public TipoVeiculo Tipo { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime? Saida { get; set; }
        public decimal ValorHora { get; set; }

        public TimeSpan TempoPermanencia => (Saida ?? DateTime.Now) - Entrada;

        public decimal CalcularValor()
        {
            var horas = (decimal)Math.Ceiling(TempoPermanencia.TotalHours);
            return horas * ValorHora;
        }
    }
}
