using Estacionamento.Models;
using Estacionamento.Repositories;
using System;

namespace Estacionamento.Services
{
    public class EstacionamentoService
    {
        private readonly Repositories.VeiculoRepositorySQLite _repository = new Repositories.VeiculoRepositorySQLite();

        public void RegistrarEntrada(string placa, TipoVeiculo tipo, decimal valorHora)
        {
            if (string.IsNullOrWhiteSpace(placa))
                throw new ArgumentException("A placa não pode ser vazia.");

            if (valorHora <= 0)
                throw new ArgumentException("O valor por hora deve ser positivo.");

            var existenteAtivo = _repository.ObterPorPlaca(placa);
            if (existenteAtivo != null)
                throw new InvalidOperationException("Já existe um registro ativo com esta placa. Finalize ou cancele antes de registrar nova entrada.");

            var novoVeiculo = new Veiculo
            {
                Placa = placa.ToUpper(),
                Tipo = tipo,
                Entrada = DateTime.Now,
                ValorHora = valorHora
            };

            _repository.Adicionar(novoVeiculo);
        }

        public Veiculo RegistrarSaida(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                throw new ArgumentException("A placa não pode ser vazia.");

            var veiculo = _repository.ObterPorPlaca(placa);

            if (veiculo == null)
                throw new InvalidOperationException("Veículo não encontrado ou já foi registrada a saída.");

            veiculo.Saida = DateTime.Now;
            _repository.Atualizar(veiculo);

            return veiculo;
        }

        public void AlterarDadosVeiculo(string placa, TipoVeiculo novoTipo, decimal novoValorHora)
        {
            if (string.IsNullOrWhiteSpace(placa))
                throw new ArgumentException("A placa não pode ser vazia.");
            if (novoValorHora <= 0)
                throw new ArgumentException("O valor por hora deve ser positivo.");

            var veiculo = _repository.ObterPorPlaca(placa);
            if (veiculo == null)
                throw new InvalidOperationException("Veículo não encontrado ou já finalizado.");

            _repository.AtualizarDados(placa, novoTipo, novoValorHora);
        }

        public void CancelarVeiculoAtivo(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                throw new ArgumentException("A placa não pode ser vazia.");
            var veiculo = _repository.ObterPorPlaca(placa);
            if (veiculo == null)
                throw new InvalidOperationException("Veículo não encontrado ou já finalizado.");
            _repository.RemoverAtivoPorPlaca(placa);
        }

        public System.Collections.Generic.List<Veiculo> ListarVeiculosEstacionados()
        {
            return _repository.ObterTodos();
        }

        public System.Collections.Generic.List<Veiculo> ListarTodosVeiculos()
        {
            return _repository.ObterTodos();
        }

        public System.Collections.Generic.List<Veiculo> ListarVeiculosAtivos()
        {
            return _repository.ObterTodos().Where(v => v.Saida == null).ToList();
        }

        public decimal ObterFaturamentoTotal()
        {
            return _repository.ObterTodos()
                .Where(v => v.Saida != null)
                .Sum(v => v.CalcularValor());
        }

        public void ExcluirVeiculoFinalizado(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                throw new ArgumentException("A placa não pode ser vazia.");
            var veiculo = _repository.ObterTodos().FirstOrDefault(v => v.Placa.Equals(placa, StringComparison.OrdinalIgnoreCase) && v.Saida != null);
            if (veiculo == null)
                throw new InvalidOperationException("Registro finalizado não encontrado.");
            _repository.RemoverFinalizadoPorPlaca(placa);
        }
    }
}
