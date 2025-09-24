

namespace Estacionamento
{
    partial class Form1
    {
    private System.Windows.Forms.Button btnGerarRelatorio;
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtPlaca;
        private System.Windows.Forms.ComboBox cmbTipoVeiculo;
        private System.Windows.Forms.TextBox txtValorHora;
        private System.Windows.Forms.Button btnRegistrarEntrada;
        private System.Windows.Forms.Button btnRegistrarSaida;
        private System.Windows.Forms.Button btnAlterarDados;
        private System.Windows.Forms.Button btnCancelarAtivo;
        private System.Windows.Forms.Button btnExcluirFinalizado;
        private System.Windows.Forms.DataGridView dgvVeiculos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox groupBoxDados;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip cmsGrid;
        private System.Windows.Forms.ToolStripMenuItem miExcluirFinalizado;
        private System.Windows.Forms.ToolStripMenuItem miCancelarAtivo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // Instanciar todos os controles primeiro
            this.btnGerarRelatorio = new System.Windows.Forms.Button();
            this.txtPlaca = new System.Windows.Forms.TextBox();
            this.cmbTipoVeiculo = new System.Windows.Forms.ComboBox();
            this.txtValorHora = new System.Windows.Forms.TextBox();
            this.btnRegistrarEntrada = new System.Windows.Forms.Button();
            this.btnRegistrarSaida = new System.Windows.Forms.Button();
            this.dgvVeiculos = new System.Windows.Forms.DataGridView();
            this.btnAlterarDados = new System.Windows.Forms.Button();
            this.btnCancelarAtivo = new System.Windows.Forms.Button();
            this.btnExcluirFinalizado = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.groupBoxDados = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.cmsGrid = new System.Windows.Forms.ContextMenuStrip();
            this.miExcluirFinalizado = new System.Windows.Forms.ToolStripMenuItem();
            this.miCancelarAtivo = new System.Windows.Forms.ToolStripMenuItem();

            ((System.ComponentModel.ISupportInitialize)(this.dgvVeiculos)).BeginInit();
            this.groupBoxDados.SuspendLayout();
            this.cmsGrid.SuspendLayout();
            this.SuspendLayout();

            // btnGerarRelatorio
            this.btnGerarRelatorio.Location = new System.Drawing.Point(520, 185);
            this.btnGerarRelatorio.Size = new System.Drawing.Size(150, 70);
            this.btnGerarRelatorio.Text = "Gerar Relatório (PDF)";
            this.btnGerarRelatorio.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnGerarRelatorio.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGerarRelatorio.Image = System.Drawing.SystemIcons.WinLogo.ToBitmap();
            this.btnGerarRelatorio.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnGerarRelatorio.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.btnGerarRelatorio, "Exporta relatório completo em PDF");
            this.btnGerarRelatorio.Click += new System.EventHandler(this.btnGerarRelatorio_Click);

            // lblTitulo
            this.lblTitulo.Text = "SISTEMA DE ESTACIONAMENTO";
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitulo.Height = 40;
            this.lblTitulo.BackColor = System.Drawing.Color.SteelBlue;
            this.lblTitulo.ForeColor = System.Drawing.Color.White;

            // groupBoxDados
            this.groupBoxDados.Controls.Add(this.txtPlaca);
            this.groupBoxDados.Controls.Add(this.cmbTipoVeiculo);
            this.groupBoxDados.Controls.Add(this.txtValorHora);
            this.groupBoxDados.Controls.Add(this.label1);
            this.groupBoxDados.Controls.Add(this.label2);
            this.groupBoxDados.Controls.Add(this.label3);
            // adiciona os botões dentro do groupBox
            this.groupBoxDados.Controls.Add(this.btnRegistrarEntrada);
            this.groupBoxDados.Controls.Add(this.btnRegistrarSaida);
            this.groupBoxDados.Controls.Add(this.btnAlterarDados);
            this.groupBoxDados.Controls.Add(this.btnCancelarAtivo);
            this.groupBoxDados.Location = new System.Drawing.Point(20, 190);
            this.groupBoxDados.Size = new System.Drawing.Size(460, 165);
            this.groupBoxDados.Text = "Dados do Veículo";

            // label1
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.Text = "Placa:";

            // txtPlaca
            this.txtPlaca.Location = new System.Drawing.Point(90, 19);
            this.txtPlaca.Size = new System.Drawing.Size(190, 20);

            // label2
            this.label2.Location = new System.Drawing.Point(10, 48);
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.Text = "Tipo:";

            // cmbTipoVeiculo
            this.cmbTipoVeiculo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoVeiculo.Location = new System.Drawing.Point(90, 45);
            this.cmbTipoVeiculo.Size = new System.Drawing.Size(190, 21);

            // label3
            this.label3.Location = new System.Drawing.Point(10, 74);
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.Text = "Valor Hora:";

            // txtValorHora
            this.txtValorHora.Location = new System.Drawing.Point(90, 71);
            this.txtValorHora.Size = new System.Drawing.Size(190, 20);

            // btnRegistrarEntrada
            this.btnRegistrarEntrada.Location = new System.Drawing.Point(330, 18);
            this.btnRegistrarEntrada.Size = new System.Drawing.Size(120, 30);
            this.btnRegistrarEntrada.Text = "     Registrar Entrada";
            this.btnRegistrarEntrada.BackColor = System.Drawing.Color.LightGreen;
            this.btnRegistrarEntrada.Image = System.Drawing.SystemIcons.Information.ToBitmap();
            this.btnRegistrarEntrada.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRegistrarEntrada.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRegistrarEntrada.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolTip1.SetToolTip(this.btnRegistrarEntrada, "Clique para registrar a entrada do veículo");
            this.btnRegistrarEntrada.Click += new System.EventHandler(this.btnRegistrarEntrada_Click);

            // btnRegistrarSaida
            this.btnRegistrarSaida.Location = new System.Drawing.Point(330, 55);
            this.btnRegistrarSaida.Size = new System.Drawing.Size(120, 30);
            this.btnRegistrarSaida.Text = "     Saída";
            this.btnRegistrarSaida.BackColor = System.Drawing.Color.IndianRed;
            this.btnRegistrarSaida.Image = System.Drawing.SystemIcons.Error.ToBitmap();
            this.btnRegistrarSaida.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRegistrarSaida.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRegistrarSaida.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolTip1.SetToolTip(this.btnRegistrarSaida, "Clique para registrar a saída do veículo");
            this.btnRegistrarSaida.Click += new System.EventHandler(this.BtnRegistrarSaida_Click);

            // btnAlterarDados
            this.btnAlterarDados.Location = new System.Drawing.Point(330, 92);
            this.btnAlterarDados.Size = new System.Drawing.Size(120, 30);
            this.btnAlterarDados.Text = "     Alterar";
            this.btnAlterarDados.BackColor = System.Drawing.Color.Khaki;
            this.btnAlterarDados.Image = System.Drawing.SystemIcons.Question.ToBitmap();
            this.btnAlterarDados.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAlterarDados.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAlterarDados.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolTip1.SetToolTip(this.btnAlterarDados, "Alterar tipo/valor do veículo ativo pela placa");
            this.btnAlterarDados.Click += new System.EventHandler(this.BtnAlterarDados_Click);

            // btnCancelarAtivo
            // dentro do groupBox, alinhado à coluna de botões
            this.btnCancelarAtivo.Location = new System.Drawing.Point(330, 129);
            this.btnCancelarAtivo.Size = new System.Drawing.Size(120, 30);
            this.btnCancelarAtivo.Text = "     Cancelar Ativo";
            this.btnCancelarAtivo.BackColor = System.Drawing.Color.LightCoral;
            this.btnCancelarAtivo.Image = System.Drawing.SystemIcons.Hand.ToBitmap();
            this.btnCancelarAtivo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelarAtivo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelarAtivo.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolTip1.SetToolTip(this.btnCancelarAtivo, "Cancelar/Excluir veículo ativo pela placa");
            this.btnCancelarAtivo.Click += new System.EventHandler(this.BtnCancelarAtivo_Click);

            // btnExcluirFinalizado
            this.btnExcluirFinalizado.Location = new System.Drawing.Point(520, 225);
            this.btnExcluirFinalizado.Size = new System.Drawing.Size(150, 35);
            this.btnExcluirFinalizado.Text = "     Excluir Finalizado";
            this.btnExcluirFinalizado.BackColor = System.Drawing.Color.LightGray;
            this.btnExcluirFinalizado.Image = System.Drawing.SystemIcons.Shield.ToBitmap();
            this.btnExcluirFinalizado.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExcluirFinalizado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExcluirFinalizado.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolTip1.SetToolTip(this.btnExcluirFinalizado, "Excluir registro já finalizado pela placa");
            this.btnExcluirFinalizado.Click += new System.EventHandler(this.BtnExcluirFinalizado_Click);

            // dgvVeiculos
            this.dgvVeiculos.Location = new System.Drawing.Point(20, 320);
            this.dgvVeiculos.Size = new System.Drawing.Size(1160, 380);
            this.dgvVeiculos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVeiculos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVeiculos.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvVeiculos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.toolTip1.SetToolTip(this.dgvVeiculos, "Lista de veículos estacionados");
            this.dgvVeiculos.ContextMenuStrip = this.cmsGrid;
            this.dgvVeiculos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DgvVeiculos_KeyDown);
            this.dgvVeiculos.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvVeiculos_CellMouseDown);

            // cmsGrid
            this.cmsGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.miExcluirFinalizado,
                this.miCancelarAtivo
            });

            // miExcluirFinalizado
            this.miExcluirFinalizado.Text = "Excluir Finalizado";
            this.miExcluirFinalizado.Click += new System.EventHandler(this.MiExcluirFinalizado_Click);

            // miCancelarAtivo
            this.miCancelarAtivo.Text = "Cancelar Ativo";
            this.miCancelarAtivo.Click += new System.EventHandler(this.MiCancelarAtivo_Click);

            // Form1
            this.ClientSize = new System.Drawing.Size(1200, 720);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.groupBoxDados);
            // botões passam a ser filhos do groupBox (não adiciona diretamente ao Form)
            this.Controls.Add(this.btnGerarRelatorio);
            this.Controls.Add(this.btnExcluirFinalizado);
            this.Controls.Add(this.dgvVeiculos);
            this.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.Name = "Form1";
            this.Text = "Sistema de Estacionamento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.dgvVeiculos)).EndInit();
            this.groupBoxDados.ResumeLayout(false);
            this.groupBoxDados.PerformLayout();
            this.cmsGrid.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
