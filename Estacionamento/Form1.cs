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
        private ComboBox cmbFiltroTipo = null!;
        private CheckBox chkApenasAtivos = null!;
        private Button btnSomenteAtivos = null!;
        private Button btnMostrarTodos = null!;
        private Panel panelGraficos = null!;
        private Chart chartFaturamento = null!;
        private Chart chartTiposVeiculos = null!;
        private ToolTip toolTip = null!;

        public Form1()
        {
            InitializeComponent();

            // Configurações do formulário
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 9);
            this.MinimumSize = new Size(1100, 720);

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
            panelDashboard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

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
                TryParseValorHora(txtValorHora.Text, out decimal valorHora))
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
                Location = new Point(20, 370),
                Size = new Size(1160, 50),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblFiltro = new Label
            {
                Text = "Placa:",
                Location = new Point(10, 15),
                Size = new Size(60, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            txtFiltroPlaca = new TextBox
            {
                Location = new Point(70, 12),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblTipo = new Label
            {
                Text = "Tipo:",
                Location = new Point(280, 15),
                Size = new Size(45, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            cmbFiltroTipo = new ComboBox
            {
                Location = new Point(330, 12),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmbFiltroTipo.Items.AddRange(new object[] { "Todos", nameof(TipoVeiculo.Carro), nameof(TipoVeiculo.Moto), nameof(TipoVeiculo.Caminhao) });
            cmbFiltroTipo.SelectedIndex = 0;

            chkApenasAtivos = new CheckBox
            {
                Text = "Apenas Ativos",
                Location = new Point(490, 14),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            btnFiltrar = new Button
            {
                Text = "Filtrar",
                Location = new Point(620, 10),
                Size = new Size(110, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnFiltrar.Click += BtnFiltrar_Click;

            btnSomenteAtivos = new Button
            {
                Text = "Somente Ativos",
                Location = new Point(730, 10),
                Size = new Size(140, 30),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSomenteAtivos.Click += (s, e) =>
            {
                chkApenasAtivos.Checked = true;
                cmbFiltroTipo.SelectedIndex = 0; // Todos
                txtFiltroPlaca.Clear();
                BtnFiltrar_Click(s!, e!);
            };

            btnMostrarTodos = new Button
            {
                Text = "Mostrar Todos",
                Location = new Point(880, 10),
                Size = new Size(140, 30),
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnMostrarTodos.Click += (s, e) =>
            {
                chkApenasAtivos.Checked = false;
                cmbFiltroTipo.SelectedIndex = 0; // Todos
                txtFiltroPlaca.Clear();
                BtnFiltrar_Click(s!, e!);
            };

            panelFiltro.Controls.AddRange(new Control[] { lblFiltro, txtFiltroPlaca, lblTipo, cmbFiltroTipo, chkApenasAtivos, btnFiltrar, btnSomenteAtivos, btnMostrarTodos });
            this.Controls.Add(panelFiltro);
        }

        private void CriarSecaoGraficos()
        {
            // Painel de gráficos - posicionado abaixo dos controles do Designer
            panelGraficos = new Panel
            {
                // Alinha depois da coluna de botões (x=360 + 140 largura + 20 espaço = 520; outra coluna 150 -> 670; +20 = 690)
                Location = new Point(690, 170),
                // Altura menor para caber filtro abaixo
                Size = new Size(490, 110),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panelGraficos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            var lblTitulo = new Label
            {
                Text = "Análises e Relatórios",
                Location = new Point(10, 8),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            // Gráfico de faturamento
            chartFaturamento = new Chart
            {
                Location = new Point(10, 30),
                Size = new Size(220, 70),
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
                Location = new Point(250, 30),
                Size = new Size(220, 70),
                BackColor = Color.White
            };

            var chartArea2 = new ChartArea("TiposArea");
            chartArea2.BackColor = Color.White;
            chartTiposVeiculos.ChartAreas.Add(chartArea2);

            var series2 = new Series("Tipos");
            series2.ChartType = SeriesChartType.Pie;
            chartTiposVeiculos.Series.Add(series2);
            // Evita rótulos sobrepostos no gráfico de pizza
            chartTiposVeiculos.Legends.Clear();
            chartTiposVeiculos.Legends.Add(new Legend("Legenda") { Docking = Docking.Bottom, Alignment = StringAlignment.Center });
            series2.IsValueShownAsLabel = false;
            series2["PieLabelStyle"] = "Disabled";

            var title2 = new Title("Distribuição de Veículos");
            title2.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            title2.ForeColor = Color.FromArgb(52, 73, 94);
            chartTiposVeiculos.Titles.Add(title2);

            panelGraficos.Controls.AddRange(new Control[] { lblTitulo, chartFaturamento, chartTiposVeiculos });
            panelGraficos.Resize += (s, e) => AtualizarLayoutGraficos();
            this.Controls.Add(panelGraficos);
            AtualizarLayoutGraficos();
        }

        private void AjustarDataGridView()
        {
            // Ajustar posição e tamanho do DataGridView (abaixo do filtro)
            dgvVeiculos.Location = new Point(20, 430);
            dgvVeiculos.Size = new Size(1160, 210);
            dgvVeiculos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvVeiculos.BackColor = Color.White;
            dgvVeiculos.BorderStyle = BorderStyle.FixedSingle;
            dgvVeiculos.AllowUserToAddRows = false;
            dgvVeiculos.ReadOnly = true;
            dgvVeiculos.CellContentClick += DgvVeiculos_CellContentClick;
            dgvVeiculos.DataBindingComplete += DgvVeiculos_DataBindingComplete;
            dgvVeiculos.CellPainting += DgvVeiculos_CellPainting;
        }

        private void AtualizarLayoutGraficos()
        {
            if (panelGraficos == null || chartFaturamento == null || chartTiposVeiculos == null) return;
            int padding = 10;
            int gap = 20;
            int header = 30; // altura do título
            int innerWidth = Math.Max(0, panelGraficos.ClientSize.Width - 2 * padding);
            int innerHeight = Math.Max(0, panelGraficos.ClientSize.Height - (header + padding));
            int chartWidth = Math.Max(120, (innerWidth - gap) / 2);

            chartFaturamento.Location = new Point(padding, header);
            chartFaturamento.Size = new Size(chartWidth, innerHeight);

            chartTiposVeiculos.Location = new Point(padding + chartWidth + gap, header);
            chartTiposVeiculos.Size = new Size(chartWidth, innerHeight);
        }

        private void DgvVeiculos_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvVeiculos.ClearSelection();
                dgvVeiculos.Rows[e.RowIndex].Selected = true;
                dgvVeiculos.CurrentCell = dgvVeiculos.Rows[e.RowIndex].Cells[e.ColumnIndex >= 0 ? e.ColumnIndex : 0];
            }
        }

        private void DgvVeiculos_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var placa = ObterPlacaSelecionadaNoGrid();
                if (string.IsNullOrEmpty(placa)) return;
                // tenta excluir finalizado; se não existir, oferece cancelar ativo
                try
                {
                    _estacionamentoService.ExcluirVeiculoFinalizado(placa);
                    AtualizarGrid();
                    AtualizarDashboard();
                    AtualizarGraficos();
                    MessageBox.Show("Registro finalizado excluído.");
                }
                catch
                {
                    var confirma = MessageBox.Show("Registro ativo encontrado. Deseja cancelar?", "Cancelar Ativo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirma == DialogResult.Yes)
                    {
                        try
                        {
                            _estacionamentoService.CancelarVeiculoAtivo(placa);
                            AtualizarGrid();
                            AtualizarDashboard();
                            AtualizarGraficos();
                            MessageBox.Show("Ativo cancelado.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void MiExcluirFinalizado_Click(object? sender, EventArgs e)
        {
            var placa = ObterPlacaSelecionadaNoGrid();
            if (string.IsNullOrEmpty(placa)) return;
            var confirm = MessageBox.Show("Excluir definitivamente o registro finalizado desta placa?", "Excluir Finalizado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;
            try
            {
                _estacionamentoService.ExcluirVeiculoFinalizado(placa);
                AtualizarGrid();
                AtualizarDashboard();
                AtualizarGraficos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MiCancelarAtivo_Click(object? sender, EventArgs e)
        {
            var placa = ObterPlacaSelecionadaNoGrid();
            if (string.IsNullOrEmpty(placa)) return;
            var confirm = MessageBox.Show("Cancelar/Excluir veículo ativo desta placa?", "Cancelar Ativo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;
            try
            {
                _estacionamentoService.CancelarVeiculoAtivo(placa);
                AtualizarGrid();
                AtualizarDashboard();
                AtualizarGraficos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObterPlacaSelecionadaNoGrid()
        {
            if (dgvVeiculos.SelectedRows.Count > 0)
            {
                var row = dgvVeiculos.SelectedRows[0];
                return row.Cells["Placa"]?.Value?.ToString() ?? string.Empty;
            }
            if (dgvVeiculos.CurrentRow != null)
            {
                return dgvVeiculos.CurrentRow.Cells["Placa"]?.Value?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        private void BtnFiltrar_Click(object? sender, EventArgs e)
        {
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

                if (!TryParseValorHora(txtValorHora.Text, out decimal valorHora) || valorHora <= 0)
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
                var horasTotais = veiculo.TempoPermanencia.TotalHours;
                var resultado = MessageBox.Show(
                    $"Confirma a saída do veículo?\n\n" +
                    $"Placa: {veiculo.Placa}\n" +
                    $"Tipo: {veiculo.Tipo}\n" +
                    $"Tempo: {horasTotais:F2} horas\n" +
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

        private void BtnAlterarDados_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPlaca.Text))
                {
                    MessageBox.Show("Digite a placa para alterar.");
                    return;
                }
                if (cmbTipoVeiculo.SelectedItem is not TipoVeiculo novoTipo)
                {
                    MessageBox.Show("Selecione o tipo de veículo.");
                    return;
                }
                if (!TryParseValorHora(txtValorHora.Text, out decimal novoValorHora) || novoValorHora <= 0)
                {
                    MessageBox.Show("Informe um valor por hora válido.");
                    return;
                }

                _estacionamentoService.AlterarDadosVeiculo(txtPlaca.Text.ToUpper(), novoTipo, novoValorHora);
                AtualizarGrid();
                AtualizarDashboard();
                AtualizarGraficos();
                MessageBox.Show("Dados alterados com sucesso.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao alterar dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelarAtivo_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPlaca.Text))
                {
                    MessageBox.Show("Digite a placa para cancelar.");
                    return;
                }

                var confirm = MessageBox.Show("Confirmar cancelamento/exclusão do veículo ativo?", "Cancelar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes) return;

                _estacionamentoService.CancelarVeiculoAtivo(txtPlaca.Text.ToUpper());
                txtPlaca.Clear();
                AtualizarGrid();
                AtualizarDashboard();
                AtualizarGraficos();
                MessageBox.Show("Cancelado com sucesso.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao cancelar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarGrid()
        {
            var veiculos = _estacionamentoService.ListarVeiculosEstacionados();
            // Filtros
            if (txtFiltroPlaca != null)
            {
                var placaFiltro = txtFiltroPlaca.Text?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(placaFiltro))
                {
                    veiculos = veiculos.Where(v => v.Placa.Contains(placaFiltro, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }
            if (cmbFiltroTipo != null && cmbFiltroTipo.SelectedIndex > 0)
            {
                var tipoTexto = cmbFiltroTipo.SelectedItem?.ToString();
                if (Enum.TryParse<TipoVeiculo>(tipoTexto, out var tipoSel))
                {
                    veiculos = veiculos.Where(v => v.Tipo == tipoSel).ToList();
                }
            }
            if (chkApenasAtivos != null && chkApenasAtivos.Checked)
            {
                veiculos = veiculos.Where(v => v.Saida == null).ToList();
            }
            dgvVeiculos.DataSource = null;
            dgvVeiculos.Columns.Clear();
            dgvVeiculos.DataSource = veiculos.Select(v => new
            {
                v.Placa,
                v.Tipo,
                Status = v.Saida != null ? "FINALIZADO" : "ATIVO",
                Entrada = v.Entrada.ToString("HH:mm:ss"),
                Saída = v.Saida?.ToString("HH:mm:ss") ?? "-",
                Tempo = v.Saida != null ? v.TempoPermanencia.ToString(@"hh\:mm") : (DateTime.Now - v.Entrada).ToString(@"hh\:mm"),
                Valor = v.Saida != null ? $"R$ {v.CalcularValor():F2}" : 
                        TryParseValorHora(txtValorHora.Text, out decimal valorHora) ?
                        $"R$ {CalcularValorEstimado(DateTime.Now - v.Entrada, valorHora):F2}" : "R$ 0,00",
                Finalizado = v.Saida != null
            }).ToList();
            dgvVeiculos.AutoGenerateColumns = true;
            dgvVeiculos.MultiSelect = false;
            dgvVeiculos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVeiculos.RowHeadersVisible = false;
            EnsureGridActionColumns();
            DgvVeiculos_DataBindingComplete(this, new DataGridViewBindingCompleteEventArgs(System.ComponentModel.ListChangedType.Reset));
        }

        private void EnsureGridActionColumns()
        {
            if (dgvVeiculos.Columns["colExcluir"] == null)
            {
                var colExcluir = new DataGridViewButtonColumn
                {
                    Name = "colExcluir",
                    HeaderText = "Excluir Finalizado",
                    Text = "Excluir",
                    UseColumnTextForButtonValue = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
                };
                dgvVeiculos.Columns.Add(colExcluir);
            }
            if (dgvVeiculos.Columns["colCancelar"] == null)
            {
                var colCancelar = new DataGridViewButtonColumn
                {
                    Name = "colCancelar",
                    HeaderText = "Cancelar Ativo",
                    Text = "Cancelar",
                    UseColumnTextForButtonValue = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
                };
                dgvVeiculos.Columns.Add(colCancelar);
            }
        }

        private void DgvVeiculos_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var column = dgvVeiculos.Columns[e.ColumnIndex];
            var placa = dgvVeiculos.Rows[e.RowIndex].Cells["Placa"]?.Value?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(placa)) return;
            var isFinalizado = false;
            if (dgvVeiculos.Rows[e.RowIndex].Cells["Finalizado"] != null)
            {
                bool.TryParse(dgvVeiculos.Rows[e.RowIndex].Cells["Finalizado"].Value?.ToString(), out isFinalizado);
            }

            if (column.Name == "colExcluir")
            {
                if (!isFinalizado) return; // só exclui finalizados
                var confirm = MessageBox.Show("Excluir definitivamente o registro finalizado desta placa?", "Excluir Finalizado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes) return;
                try
                {
                    _estacionamentoService.ExcluirVeiculoFinalizado(placa);
                    AtualizarGrid();
                    AtualizarDashboard();
                    AtualizarGraficos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (column.Name == "colCancelar")
            {
                if (isFinalizado) return; // só cancela ativos
                var confirm = MessageBox.Show("Cancelar/Excluir veículo ativo desta placa?", "Cancelar Ativo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes) return;
                try
                {
                    _estacionamentoService.CancelarVeiculoAtivo(placa);
                    AtualizarGrid();
                    AtualizarDashboard();
                    AtualizarGraficos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DgvVeiculos_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Garante que as colunas de ação existam antes de manipular células
            if (dgvVeiculos.Columns["colExcluir"] == null || dgvVeiculos.Columns["colCancelar"] == null)
            {
                EnsureGridActionColumns();
                if (dgvVeiculos.Columns["colExcluir"] == null || dgvVeiculos.Columns["colCancelar"] == null)
                {
                    return;
                }
            }
            // Ajusta habilitação dos botões por linha conforme Finalizado
            foreach (DataGridViewRow row in dgvVeiculos.Rows)
            {
                var isFinalizado = false;
                if (row.Cells["Finalizado"] != null)
                {
                    bool.TryParse(row.Cells["Finalizado"].Value?.ToString(), out isFinalizado);
                }

                var excluirCell = row.Cells["colExcluir"] as DataGridViewButtonCell;
                var cancelarCell = row.Cells["colCancelar"] as DataGridViewButtonCell;
                if (excluirCell != null)
                {
                    excluirCell.FlatStyle = FlatStyle.Standard;
                    excluirCell.Style.ForeColor = isFinalizado ? Color.Black : Color.LightGray;
                    excluirCell.Value = isFinalizado ? "Excluir" : "";
                }
                if (cancelarCell != null)
                {
                    cancelarCell.FlatStyle = FlatStyle.Standard;
                    cancelarCell.Style.ForeColor = isFinalizado ? Color.LightGray : Color.Black;
                    cancelarCell.Value = isFinalizado ? "" : "Cancelar";
                }

                // Cores por status
                if (isFinalizado)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240); // cinza claro
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
                    row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 200, 200);
                    row.DefaultCellStyle.SelectionForeColor = Color.Black;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(230, 245, 233); // verde bem claro
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(40, 70, 40);
                    row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 230, 200);
                    row.DefaultCellStyle.SelectionForeColor = Color.Black;
                }

                // Cor do texto na coluna Status
                if (row.Cells["Status"] != null)
                {
                    var statusValor = row.Cells["Status"].Value?.ToString() ?? string.Empty;
                    if (string.Equals(statusValor, "ATIVO", StringComparison.OrdinalIgnoreCase))
                    {
                        row.Cells["Status"].Style.ForeColor = Color.FromArgb(0, 100, 0);
                    }
                    else
                    {
                        row.Cells["Status"].Style.ForeColor = Color.FromArgb(100, 100, 100);
                    }
                }
            }

            // Coluna Status como primeira e em negrito
            if (dgvVeiculos.Columns["Status"] != null)
            {
                dgvVeiculos.Columns["Status"].DisplayIndex = 0;
                dgvVeiculos.Columns["Status"].HeaderText = "Status";
                dgvVeiculos.Columns["Status"].DefaultCellStyle.Font = new Font(dgvVeiculos.Font, FontStyle.Bold);
                dgvVeiculos.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvVeiculos.Columns["Status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void DgvVeiculos_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var column = dgvVeiculos.Columns[e.ColumnIndex];
            if (column == null || (column.Name != "colExcluir" && column.Name != "colCancelar")) return;

            var isFinalizado = false;
            if (dgvVeiculos.Rows[e.RowIndex].Cells["Finalizado"] != null)
            {
                bool.TryParse(dgvVeiculos.Rows[e.RowIndex].Cells["Finalizado"].Value?.ToString(), out isFinalizado);
            }

            var deveOcultar = (column.Name == "colExcluir" && !isFinalizado) || (column.Name == "colCancelar" && isFinalizado);
            if (!deveOcultar) return;

            // Pinta apenas o fundo e bordas, sem conteúdo (oculta o botão visualmente)
            e.PaintBackground(e.CellBounds, true);
            e.Handled = true;
        }

        private bool TryParseValorHora(string input, out decimal value)
        {
            input = (input ?? string.Empty).Trim();
            // Tenta pt-BR (vírgula decimal)
            if (decimal.TryParse(input, System.Globalization.NumberStyles.Number, new System.Globalization.CultureInfo("pt-BR"), out value))
                return true;
            // Tenta Invariant (ponto decimal)
            if (decimal.TryParse(input.Replace(',', '.'), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out value))
                return true;
            return false;
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

        private void BtnExcluirFinalizado_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPlaca.Text))
                {
                    MessageBox.Show("Digite a placa para excluir finalizado.");
                    return;
                }

                var confirm = MessageBox.Show("Excluir definitivamente o registro finalizado desta placa?", "Excluir Finalizado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes) return;

                _estacionamentoService.ExcluirVeiculoFinalizado(txtPlaca.Text.ToUpper());
                txtPlaca.Clear();
                AtualizarGrid();
                AtualizarDashboard();
                AtualizarGraficos();
                MessageBox.Show("Registro finalizado excluído.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao excluir finalizado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
