using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Estacionamento
{
    public partial class Form1 : Form
    {
        private List<Veiculo> veiculos = new List<Veiculo>();

        public Form1()
        {
            InitializeComponent();
            cmbTipoVeiculo.Items.AddRange(new string[] { "Carro", "Moto", "Caminhão" });
            cmbTipoVeiculo.SelectedIndex = 0;
            AtualizarGrid();
        }

        private void btnRegistrarEntrada_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlaca.Text))
            {
                MessageBox.Show("Informe a placa do veículo.");
                return;
            }

            if (!double.TryParse(txtValorHora.Text, out double valorHora) || valorHora <= 0)
            {
                MessageBox.Show("Informe um valor por hora válido.");
                return;
            }

            veiculos.Add(new Veiculo
            {
                Placa = txtPlaca.Text.ToUpper(),
                Tipo = cmbTipoVeiculo.SelectedItem.ToString(),
                Entrada = DateTime.Now,
                ValorHora = valorHora
            });

            txtPlaca.Clear();
            txtValorHora.Clear();
            AtualizarGrid();
        }

        private void btnRegistrarSaida_Click(object sender, EventArgs e)
        {
            string placa = txtPlaca.Text.ToUpper();
            var veiculo = veiculos.FirstOrDefault(v => v.Placa == placa && v.Saida == null);

            if (veiculo != null)
            {
                veiculo.Saida = DateTime.Now;
                double valor = veiculo.CalcularValor();

                MessageBox.Show($"Saída registrada!\nTempo: {veiculo.TempoPermanencia.TotalMinutes:F0} minutos\nValor: R$ {valor:F2}");
                AtualizarGrid();
            }
            else
            {
                MessageBox.Show("Veículo não encontrado ou já saiu.");
            }
        }

        private void AtualizarGrid()
        {
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
