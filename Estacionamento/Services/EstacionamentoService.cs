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

            if (_repository.ObterPorPlaca(placa) != null)
                throw new InvalidOperationException("Veículo com esta placa já está no estacionamento.");

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

        public System.Collections.Generic.List<Veiculo> ListarVeiculosEstacionados()
        {
            return _repository.ObterTodos();
        }
    }
}
