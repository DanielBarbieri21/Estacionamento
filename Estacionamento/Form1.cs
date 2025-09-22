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
            if (cmbTipoVeiculo != null)
                cmbTipoVeiculo.DataSource = Enum.GetValues(typeof(TipoVeiculo));
            else
                MessageBox.Show("Erro ao inicializar o controle de tipo de veículo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            AtualizarGrid();
        }

        private void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                var veiculos = _estacionamentoService.ListarVeiculosEstacionados();
                if (veiculos.Count == 0)
                {
                    MessageBox.Show("Nenhum veículo registrado.");
                    return;
                }

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "PDF Files|*.pdf";
                    sfd.Title = "Salvar Relatório PDF";
                    sfd.FileName = "Relatorio_Estacionamento.pdf";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        GerarRelatorioPDF(veiculos, sfd.FileName);
                        MessageBox.Show("Relatório gerado com sucesso!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar relatório: {ex.Message}");
            }

        }

        private void GerarRelatorioPDF(System.Collections.Generic.List<Estacionamento.Models.Veiculo> veiculos, string filePath)
        {
            using (var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                var doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);
                var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs);
                doc.Open();

                var titulo = new iTextSharp.text.Paragraph("Relatório de Estacionamento", new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 18, iTextSharp.text.Font.BOLD));
                titulo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new iTextSharp.text.Paragraph($"Data: {DateTime.Now:dd/MM/yyyy HH:mm}", new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 12)));
                doc.Add(new iTextSharp.text.Paragraph("\n"));

                var tabela = new iTextSharp.text.pdf.PdfPTable(6);
                tabela.WidthPercentage = 100;
                tabela.AddCell("Placa");
                tabela.AddCell("Tipo");
                tabela.AddCell("Entrada");
                tabela.AddCell("Saída");
                tabela.AddCell("Valor/Hora");
                tabela.AddCell("Valor Total");

                decimal faturamento = 0;
                foreach (var v in veiculos)
                {
                    tabela.AddCell(v.Placa);
                    tabela.AddCell(v.Tipo.ToString());
                    tabela.AddCell(v.Entrada.ToString("dd/MM/yyyy HH:mm"));
                    tabela.AddCell(v.Saida?.ToString("dd/MM/yyyy HH:mm") ?? "-");
                    tabela.AddCell($"R$ {v.ValorHora:F2}");
                    var valor = v.Saida != null ? v.CalcularValor() : 0;
                    tabela.AddCell($"R$ {valor:F2}");
                    faturamento += valor;
                }
                doc.Add(tabela);

                doc.Add(new iTextSharp.text.Paragraph("\n"));
                doc.Add(new iTextSharp.text.Paragraph($"Faturamento total: R$ {faturamento:F2}", new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 14, iTextSharp.text.Font.BOLD)));

                doc.Close();
                writer.Close();
            }
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

                if (cmbTipoVeiculo.SelectedItem is TipoVeiculo tipoVeiculo)
                {
                    _estacionamentoService.RegistrarEntrada(
                        txtPlaca.Text,
                        tipoVeiculo,
                        valorHora
                    );
                }
                else
                {
                    MessageBox.Show("Selecione um tipo de veículo válido.");
                    return;
                }

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
