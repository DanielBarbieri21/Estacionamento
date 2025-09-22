

namespace Estacionamento
{
    partial class Form1
    {
    private System.Windows.Forms.PictureBox pictureBoxLogo;
    private System.Windows.Forms.Button btnGerarRelatorio;
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtPlaca;
        private System.Windows.Forms.ComboBox cmbTipoVeiculo;
        private System.Windows.Forms.TextBox txtValorHora;
        private System.Windows.Forms.Button btnRegistrarEntrada;
        private System.Windows.Forms.Button btnRegistrarSaida;
        private System.Windows.Forms.DataGridView dgvVeiculos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox groupBoxDados;
        private System.Windows.Forms.ToolTip toolTip1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // pictureBoxLogo
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            string logoPath = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "Resources", "logo_parksystem.png");
            if (System.IO.File.Exists(logoPath))
            {
                try
                {
                    this.pictureBoxLogo.Image = System.Drawing.Image.FromFile(logoPath);
                }
                catch (System.OutOfMemoryException)
                {
                    System.Windows.Forms.MessageBox.Show("A imagem da logo está corrompida ou não é um arquivo válido.", "Erro ao carregar logo", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    this.pictureBoxLogo.Image = null;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Arquivo da logo não encontrado: " + logoPath, "Logo não encontrada", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                this.pictureBoxLogo.Image = null;
            }
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            // Nova posição: lado direito, centralizado verticalmente
            this.pictureBoxLogo.Location = new System.Drawing.Point(700, 120); // ajuste conforme largura do form
            this.pictureBoxLogo.Size = new System.Drawing.Size(300, 300); // tamanho maior para destaque
            this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;

            // Instanciar todos os controles primeiro
            this.btnGerarRelatorio = new System.Windows.Forms.Button();
            this.txtPlaca = new System.Windows.Forms.TextBox();
            this.cmbTipoVeiculo = new System.Windows.Forms.ComboBox();
            this.txtValorHora = new System.Windows.Forms.TextBox();
            this.btnRegistrarEntrada = new System.Windows.Forms.Button();
            this.btnRegistrarSaida = new System.Windows.Forms.Button();
            this.dgvVeiculos = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.groupBoxDados = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip();

            ((System.ComponentModel.ISupportInitialize)(this.dgvVeiculos)).BeginInit();
            this.groupBoxDados.SuspendLayout();
            this.SuspendLayout();

            // btnGerarRelatorio
            this.btnGerarRelatorio.Location = new System.Drawing.Point(500, 60);
            this.btnGerarRelatorio.Size = new System.Drawing.Size(130, 70);
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
            this.groupBoxDados.Location = new System.Drawing.Point(12, 50);
            this.groupBoxDados.Size = new System.Drawing.Size(300, 110);
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
            this.btnRegistrarEntrada.Location = new System.Drawing.Point(330, 60);
            this.btnRegistrarEntrada.Size = new System.Drawing.Size(140, 30);
            this.btnRegistrarEntrada.Text = "     Registrar Entrada";
            this.btnRegistrarEntrada.BackColor = System.Drawing.Color.LightGreen;
            this.btnRegistrarEntrada.Image = System.Drawing.SystemIcons.Information.ToBitmap();
            this.btnRegistrarEntrada.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRegistrarEntrada.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRegistrarEntrada.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolTip1.SetToolTip(this.btnRegistrarEntrada, "Clique para registrar a entrada do veículo");
            this.btnRegistrarEntrada.Click += new System.EventHandler(this.btnRegistrarEntrada_Click);

            // btnRegistrarSaida
            this.btnRegistrarSaida.Location = new System.Drawing.Point(330, 100);
            this.btnRegistrarSaida.Size = new System.Drawing.Size(140, 30);
            this.btnRegistrarSaida.Text = "     Registrar Saída";
            this.btnRegistrarSaida.BackColor = System.Drawing.Color.IndianRed;
            this.btnRegistrarSaida.Image = System.Drawing.SystemIcons.Error.ToBitmap();
            this.btnRegistrarSaida.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRegistrarSaida.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRegistrarSaida.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolTip1.SetToolTip(this.btnRegistrarSaida, "Clique para registrar a saída do veículo");
            this.btnRegistrarSaida.Click += new System.EventHandler(this.btnRegistrarSaida_Click);

            // dgvVeiculos
            this.dgvVeiculos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVeiculos.Location = new System.Drawing.Point(12, 170);
            this.dgvVeiculos.Size = new System.Drawing.Size(620, 220);
            this.dgvVeiculos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVeiculos.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvVeiculos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.toolTip1.SetToolTip(this.dgvVeiculos, "Lista de veículos estacionados");

            // Form1
            this.ClientSize = new System.Drawing.Size(650, 400);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.groupBoxDados);
            this.Controls.Add(this.btnRegistrarEntrada);
            this.Controls.Add(this.btnRegistrarSaida);
            this.Controls.Add(this.btnGerarRelatorio);
            this.Controls.Add(this.dgvVeiculos);
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Name = "Form1";
            this.Text = "Sistema de Estacionamento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.dgvVeiculos)).EndInit();
            this.groupBoxDados.ResumeLayout(false);
            this.groupBoxDados.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
