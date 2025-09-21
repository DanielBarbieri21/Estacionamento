using Estacionamento.Models;
using Estacionamento.Services;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Estacionamento
{
    public partial class Form1 : Form
    {
        private readonly EstacionamentoService _estacionamentoService = new EstacionamentoService();

        public Form1()
        {
            InitializeComponent();
            cmbTipoVeiculo.DataSource = Enum.GetValues(typeof(TipoVeiculo));
            AtualizarGrid();
        }

        private void btnRegistrarEntrada_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtValorHora.Text, out decimal valorHora))
                {
                    MessageBox.Show("Informe um valor por hora válido.");
                    return;
                }

                _estacionamentoService.RegistrarEntrada(
                    txtPlaca.Text,
                    (TipoVeiculo)cmbTipoVeiculo.SelectedItem,
                    valorHora
                );

                txtPlaca.Clear();
                txtValorHora.Clear();
                AtualizarGrid();
                MessageBox.Show("Veículo registrado com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao registrar entrada");
            }
        }

        private void btnRegistrarSaida_Click(object sender, EventArgs e)
        {
            try
            {
                var veiculo = _estacionamentoService.RegistrarSaida(txtPlaca.Text);
                decimal valor = veiculo.CalcularValor();

                MessageBox.Show($"Saída registrada!\nTempo: {veiculo.TempoPermanencia.TotalMinutes:F0} minutos\nValor: R$ {valor:F2}");
                AtualizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao registrar saída");
            }
        }

        private void AtualizarGrid()
        {
            var veiculos = _estacionamentoService.ListarVeiculosEstacionados();

            dgvVeiculos.DataSource = null;
            dgvVeiculos.DataSource = veiculos.Select(v => new
            {
                v.Placa,
                v.Tipo,
                Entrada = v.Entrada.ToString("HH:mm:ss"),
                Saída = v.Saida?.ToString("HH:mm:ss") ?? "-",
                Tempo = v.TempoPermanencia.ToString(@"hh\:mm"),
                Valor = v.Saida != null ? $"R$ {v.CalcularValor():F2}" : "-"
            }).ToList();
        }
    }
}
