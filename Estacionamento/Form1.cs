using Estacionamento.Models;
using Estacionamento.Services;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace Estacionamento
{
    public partial class Form1 : Form
    {
        private readonly EstacionamentoService _estacionamentoService = new EstacionamentoService();

        // Controles da interface moderna
        private Panel panelDashboard = null!;
        private Label lblTotalVeiculos = null!;
        private Label lblCarros = null!;
        private Label lblMotos = null!;
        private Label lblCaminhoes = null!;
        private Label lblFaturamento = null!;
        private TextBox txtFiltroPlaca = null!;
        private Button btnFiltrar = null!;
        private Chart chartFaturamento = null!;
        private Chart chartTiposVeiculos = null!;
        private ToolTip toolTip = null!;

        public Form1()
        {
            InitializeComponent();

            // Configurações do formulário
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 9);

            // ToolTip
            toolTip = new ToolTip();

            // Criar layout limpo e organizado
            CriarLayoutLimpo();
            
            // Criar filtro simples
            CriarFiltroSimples();

            // Configurar ComboBox
            if (cmbTipoVeiculo != null)
                cmbTipoVeiculo.DataSource = Enum.GetValues(typeof(TipoVeiculo));

            // Configurar eventos para cálculo em tempo real
            ConfigurarEventos();

            // Atualizar dados
            AtualizarGrid();
            AtualizarDashboard();
            AtualizarGraficos();
        }

        private void CriarLayoutLimpo()
        {
            // Dashboard no topo
            CriarDashboard();
            
            // Seção de gráficos (sem conflitar com elementos do Designer)
            CriarSecaoGraficos();
            
            // Ajustar DataGridView
            AjustarDataGridView();
        }

        private void CriarDashboard()
        {
            // Painel principal do dashboard
            panelDashboard = new Panel
            {
                Location = new Point(20, 50),
                Size = new Size(1160, 130),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Caixas de estatísticas com espaçamento adequado
            int x = 15;
            int y = 15;

            // Total de Veículos
            var totalPanel = CriarCaixaEstatistica("Total Veículos", "0", Color.FromArgb(52, 152, 219), x, y);
            x += 215;

            // Carros
            var carrosPanel = CriarCaixaEstatistica("Carros", "0", Color.FromArgb(46, 204, 113), x, y);
            x += 215;

            // Motos
            var motosPanel = CriarCaixaEstatistica("Motos", "0", Color.FromArgb(241, 196, 15), x, y);
            x += 215;

            // Caminhões
            var caminhoesPanel = CriarCaixaEstatistica("Caminhões", "0", Color.FromArgb(231, 76, 60), x, y);
            x += 215;

            // Faturamento
            var faturamentoPanel = CriarCaixaEstatistica("Faturamento", "R$ 0,00", Color.FromArgb(155, 89, 182), x, y);

            // Adicionar painéis ao dashboard
            panelDashboard.Controls.AddRange(new Control[] { totalPanel, carrosPanel, motosPanel, caminhoesPanel, faturamentoPanel });
            this.Controls.Add(panelDashboard);
        }

        private Panel CriarCaixaEstatistica(string titulo, string valor, Color cor, int x, int y)
        {
            var panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(200, 90),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblTitulo = new Label
            {
                Text = titulo,
                Location = new Point(5, 5),
                Size = new Size(190, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblValor = new Label
            {
                Text = valor,
                Location = new Point(5, 30),
                Size = new Size(190, 50),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = cor,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panel.Controls.Add(lblTitulo);
            panel.Controls.Add(lblValor);

            // Armazenar referências para atualização
            switch (titulo)
            {
                case "Total Veículos":
                    lblTotalVeiculos = lblValor;
                    break;
                case "Carros":
                    lblCarros = lblValor;
                    break;
                case "Motos":
                    lblMotos = lblValor;
                    break;
                case "Caminhões":
                    lblCaminhoes = lblValor;
                    break;
                case "Faturamento":
                    lblFaturamento = lblValor;
                    break;
            }

            return panel;
        }

        private void ConfigurarEventos()
        {
            // Timer para atualizar valores em tempo real
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // Atualizar a cada segundo
            timer.Tick += Timer_Tick;
            timer.Start();

            // Evento para mostrar valor estimado quando digitar valor por hora
            txtValorHora.TextChanged += TxtValorHora_TextChanged;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Atualizar grid com tempos em tempo real
            AtualizarGrid();
            AtualizarDashboard();
            AtualizarGraficos();
        }

        private void TxtValorHora_TextChanged(object? sender, EventArgs e)
        {
            // Mostrar valor estimado se houver placa e valor por hora
            if (!string.IsNullOrEmpty(txtPlaca.Text) && 
                decimal.TryParse(txtValorHora.Text, out decimal valorHora))
            {
                var veiculos = _estacionamentoService.ListarVeiculosEstacionados();
                var veiculo = veiculos.FirstOrDefault(v => v.Placa.Equals(txtPlaca.Text, StringComparison.OrdinalIgnoreCase));
                
                if (veiculo != null)
                {
                    var tempoDecorrido = DateTime.Now - veiculo.Entrada;
                    var valorEstimado = CalcularValorEstimado(tempoDecorrido, valorHora);
                    toolTip.SetToolTip(txtValorHora, $"Valor estimado: R$ {valorEstimado:F2}");
                }
            }
        }

        private decimal CalcularValorEstimado(TimeSpan tempo, decimal valorHora)
        {
            // Calcular valor baseado no tempo decorrido
            var horas = (decimal)tempo.TotalHours;
            return Math.Ceiling(horas) * valorHora; // Arredondar para cima
        }

        private void CriarFiltroSimples()
        {
            // Painel de filtro simples
            var panelFiltro = new Panel
            {
                Location = new Point(20, 340),
                Size = new Size(650, 50),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblFiltro = new Label
            {
                Text = "Filtrar por placa:",
                Location = new Point(10, 15),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            txtFiltroPlaca = new TextBox
            {
                Location = new Point(140, 12),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnFiltrar = new Button
            {
                Text = "Filtrar",
                Location = new Point(350, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnFiltrar.Click += BtnFiltrar_Click;

            panelFiltro.Controls.AddRange(new Control[] { lblFiltro, txtFiltroPlaca, btnFiltrar });
            this.Controls.Add(panelFiltro);
        }

        private void CriarSecaoGraficos()
        {
            // Painel de gráficos - posicionado abaixo dos controles do Designer
            var panelGraficos = new Panel
            {
                Location = new Point(690, 200),
                Size = new Size(490, 180),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblTitulo = new Label
            {
                Text = "Análises e Relatórios",
                Location = new Point(10, 5),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            // Gráfico de faturamento
            chartFaturamento = new Chart
            {
                Location = new Point(10, 35),
                Size = new Size(230, 130),
                BackColor = Color.White
            };

            var chartArea1 = new ChartArea("MainArea");
            chartArea1.BackColor = Color.White;
            chartArea1.AxisX.Title = "Data";
            chartArea1.AxisY.Title = "Valor (R$)";
            chartFaturamento.ChartAreas.Add(chartArea1);

            var series1 = new Series("Faturamento");
            series1.ChartType = SeriesChartType.Column;
            series1.Color = Color.FromArgb(52, 152, 219);
            chartFaturamento.Series.Add(series1);

            var title1 = new Title("Faturamento Diário");
            title1.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            title1.ForeColor = Color.FromArgb(52, 73, 94);
            chartFaturamento.Titles.Add(title1);

            // Gráfico de tipos de veículos
            chartTiposVeiculos = new Chart
            {
                Location = new Point(250, 35),
                Size = new Size(230, 130),
                BackColor = Color.White
            };

            var chartArea2 = new ChartArea("TiposArea");
            chartArea2.BackColor = Color.White;
            chartTiposVeiculos.ChartAreas.Add(chartArea2);

            var series2 = new Series("Tipos");
            series2.ChartType = SeriesChartType.Pie;
            chartTiposVeiculos.Series.Add(series2);

            var title2 = new Title("Distribuição de Veículos");
            title2.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            title2.ForeColor = Color.FromArgb(52, 73, 94);
            chartTiposVeiculos.Titles.Add(title2);

            panelGraficos.Controls.AddRange(new Control[] { lblTitulo, chartFaturamento, chartTiposVeiculos });
            this.Controls.Add(panelGraficos);
        }

        private void AjustarDataGridView()
        {
            // Ajustar posição e tamanho do DataGridView
            dgvVeiculos.Location = new Point(20, 410);
            dgvVeiculos.Size = new Size(1160, 280);
            dgvVeiculos.BackColor = Color.White;
            dgvVeiculos.BorderStyle = BorderStyle.FixedSingle;
        }

        private void BtnFiltrar_Click(object? sender, EventArgs e)
        {
            var placaFiltro = txtFiltroPlaca.Text.Trim();
            var veiculos = _estacionamentoService.ListarVeiculosEstacionados();
            if (!string.IsNullOrEmpty(placaFiltro))
            {
                veiculos = veiculos.Where(v => v.Placa.Contains(placaFiltro, StringComparison.OrdinalIgnoreCase)).ToList();
            }
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

                var tabela =    new iTextSharp.text.pdf.PdfPTable(6);
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
                // Validações
                if (string.IsNullOrWhiteSpace(txtPlaca.Text))
                {
                    MessageBox.Show("Por favor, digite a placa do veículo.", "Placa Obrigatória", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidarPlaca(txtPlaca.Text))
                {
                    MessageBox.Show("Por favor, digite uma placa válida (formato: ABC1234 ou ABC1D23).", 
                        "Placa Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtValorHora.Text, out decimal valorHora) || valorHora <= 0)
                {
                    MessageBox.Show("Informe um valor por hora válido e maior que zero.", "Valor Inválido", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbTipoVeiculo.SelectedItem is TipoVeiculo tipoVeiculo)
                {
                    var placaFormatada = txtPlaca.Text.ToUpper();
                    
                    _estacionamentoService.RegistrarEntrada(
                        placaFormatada,
                        tipoVeiculo,
                        valorHora
                    );

                    // Limpar campos após entrada
                    txtPlaca.Clear();
                    txtValorHora.Clear();
                    cmbTipoVeiculo.SelectedIndex = 0;
                    
                    AtualizarGrid();
                    AtualizarDashboard();
                    AtualizarGraficos();
                    
                    MessageBox.Show($"Veículo {tipoVeiculo} registrado com sucesso!\nPlaca: {placaFormatada}", 
                        "Entrada Registrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Selecione um tipo de veículo válido.", "Tipo Obrigatório", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao registrar entrada", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarPlaca(string placa)
        {
            // Validar formato de placa brasileira (ABC1234 ou ABC1D23)
            var placaLimpa = placa.Replace("-", "").Replace(" ", "").ToUpper();
            return placaLimpa.Length == 7 && 
                   char.IsLetter(placaLimpa[0]) && 
                   char.IsLetter(placaLimpa[1]) && 
                   char.IsLetter(placaLimpa[2]) &&
                   char.IsDigit(placaLimpa[3]) && 
                   char.IsDigit(placaLimpa[4]) && 
                   char.IsDigit(placaLimpa[5]) && 
                   char.IsDigit(placaLimpa[6]);
        }

        private void BtnRegistrarSaida_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPlaca.Text))
                {
                    MessageBox.Show("Por favor, digite a placa do veículo.", "Placa Obrigatória", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var veiculo = _estacionamentoService.RegistrarSaida(txtPlaca.Text);
                decimal valor = veiculo.CalcularValor();

                // Modal de confirmação com detalhes
                var resultado = MessageBox.Show(
                    $"Confirma a saída do veículo?\n\n" +
                    $"Placa: {veiculo.Placa}\n" +
                    $"Tipo: {veiculo.Tipo}\n" +
                    $"Tempo: {veiculo.TempoPermanencia.TotalMinutes:F0} minutos\n" +
                    $"Valor a pagar: R$ {valor:F2}",
                    "Confirmar Saída",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    // Limpar campos após saída
                    txtPlaca.Clear();
                    txtValorHora.Clear();
                    
                    MessageBox.Show($"Saída registrada com sucesso!\nValor cobrado: R$ {valor:F2}", 
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                AtualizarGrid();
                    AtualizarDashboard();
                    AtualizarGraficos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao registrar saída", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Tempo = v.Saida != null ? v.TempoPermanencia.ToString(@"hh\:mm") : (DateTime.Now - v.Entrada).ToString(@"hh\:mm"),
                Valor = v.Saida != null ? $"R$ {v.CalcularValor():F2}" : 
                        decimal.TryParse(txtValorHora.Text, out decimal valorHora) ? 
                        $"R$ {CalcularValorEstimado(DateTime.Now - v.Entrada, valorHora):F2}" : "R$ 0,00"
            }).ToList();
        }

        private void AtualizarDashboard()
        {
            var veiculosAtivos = _estacionamentoService.ListarVeiculosAtivos();
            var todosVeiculos = _estacionamentoService.ListarTodosVeiculos();
            
            int totalAtivos = veiculosAtivos.Count;
            int carros = veiculosAtivos.Count(v => v.Tipo == TipoVeiculo.Carro);
            int motos = veiculosAtivos.Count(v => v.Tipo == TipoVeiculo.Moto);
            int caminhoes = veiculosAtivos.Count(v => v.Tipo == TipoVeiculo.Caminhao);
            decimal faturamento = _estacionamentoService.ObterFaturamentoTotal();

            lblTotalVeiculos.Text = totalAtivos.ToString();
            lblCarros.Text = carros.ToString();
            lblMotos.Text = motos.ToString();
            lblCaminhoes.Text = caminhoes.ToString();
            lblFaturamento.Text = $"R$ {faturamento:F2}";
        }

        private void AtualizarGraficos()
        {
            var veiculos = _estacionamentoService.ListarVeiculosEstacionados();

            // Atualizar gráfico de faturamento
            var dadosFaturamento = veiculos.Where(v => v.Saida != null)
                .GroupBy(v => v.Saida.Value.Date)
                .Select(g => new { Data = g.Key, Valor = g.Sum(v => v.CalcularValor()) })
                .OrderBy(g => g.Data)
                .ToList();

            chartFaturamento.Series["Faturamento"].Points.Clear();
            foreach (var d in dadosFaturamento)
            {
                chartFaturamento.Series["Faturamento"].Points.AddXY(d.Data.ToString("dd/MM"), d.Valor);
            }

            // Atualizar gráfico de tipos
            var tiposDados = veiculos.GroupBy(v => v.Tipo)
                .Select(g => new { Tipo = g.Key.ToString(), Quantidade = g.Count() })
                .ToList();

            chartTiposVeiculos.Series["Tipos"].Points.Clear();
            foreach (var t in tiposDados)
            {
                chartTiposVeiculos.Series["Tipos"].Points.AddXY(t.Tipo, t.Quantidade);
            }
        }
    }
}
