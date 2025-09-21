using Estacionamento.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Estacionamento.Repositories
{
    public class VeiculoRepository
    {
        private const string _filePath = "dados.json";
        private readonly List<Veiculo> _veiculos;

        public VeiculoRepository()
        {
            _veiculos = CarregarDados();
        }

        private List<Veiculo> CarregarDados()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Veiculo>();
            }

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Veiculo>>(json) ?? new List<Veiculo>();
        }

        public void SalvarDados()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_veiculos, options);
            File.WriteAllText(_filePath, json);
        }

        public void Adicionar(Veiculo veiculo)
        {
            _veiculos.Add(veiculo);
            SalvarDados(); 
        }

        public Veiculo? ObterPorPlaca(string placa)
        {
            return _veiculos.FirstOrDefault(v => v.Placa.Equals(placa, System.StringComparison.OrdinalIgnoreCase) && v.Saida == null);
        }

        public List<Veiculo> ObterTodos()
        {
            return _veiculos.ToList(); 
        }
        
        public void Atualizar(Veiculo veiculo)
        {
            var veiculoExistente = _veiculos.FirstOrDefault(v => v.Placa == veiculo.Placa && v.Entrada == veiculo.Entrada);
            if (veiculoExistente != null)
            {
                // Atualiza as propriedades necessarias, Saida neste caso.
                veiculoExistente.Saida = veiculo.Saida;
                SalvarDados();
            }
        }
    }
}
