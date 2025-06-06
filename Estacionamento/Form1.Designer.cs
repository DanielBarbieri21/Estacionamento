namespace Estacionamento
{
    partial class Form1
    {
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtPlaca = new System.Windows.Forms.TextBox();
            this.cmbTipoVeiculo = new System.Windows.Forms.ComboBox();
            this.txtValorHora = new System.Windows.Forms.TextBox();
            this.btnRegistrarEntrada = new System.Windows.Forms.Button();
            this.btnRegistrarSaida = new System.Windows.Forms.Button();
            this.dgvVeiculos = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvVeiculos)).BeginInit();
            this.SuspendLayout();

            // txtPlaca
            this.txtPlaca.Location = new System.Drawing.Point(80, 12);
            this.txtPlaca.Name = "txtPlaca";
            this.txtPlaca.Size = new System.Drawing.Size(120, 20);

            // cmbTipoVeiculo
            this.cmbTipoVeiculo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoVeiculo.Location = new System.Drawing.Point(80, 40);
            this.cmbTipoVeiculo.Name = "cmbTipoVeiculo";
            this.cmbTipoVeiculo.Size = new System.Drawing.Size(120, 21);

            // txtValorHora
            this.txtValorHora.Location = new System.Drawing.Point(80, 67);
            this.txtValorHora.Name = "txtValorHora";
            this.txtValorHora.Size = new System.Drawing.Size(120, 20);

            // btnRegistrarEntrada
            this.btnRegistrarEntrada.Location = new System.Drawing.Point(220, 10);
            this.btnRegistrarEntrada.Name = "btnRegistrarEntrada";
            this.btnRegistrarEntrada.Size = new System.Drawing.Size(140, 23);
            this.btnRegistrarEntrada.Text = "Registrar Entrada";
            this.btnRegistrarEntrada.Click += new System.EventHandler(this.btnRegistrarEntrada_Click);

            // btnRegistrarSaida
            this.btnRegistrarSaida.Location = new System.Drawing.Point(220, 40);
            this.btnRegistrarSaida.Name = "btnRegistrarSaida";
            this.btnRegistrarSaida.Size = new System.Drawing.Size(140, 23);
            this.btnRegistrarSaida.Text = "Registrar Saída";
            this.btnRegistrarSaida.Click += new System.EventHandler(this.btnRegistrarSaida_Click);

            // dgvVeiculos
            this.dgvVeiculos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVeiculos.Location = new System.Drawing.Point(12, 100);
            this.dgvVeiculos.Name = "dgvVeiculos";
            this.dgvVeiculos.Size = new System.Drawing.Size(640, 230);
            this.dgvVeiculos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // label1
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Text = "Placa:";

            // label2
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Text = "Tipo:";

            // label3
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Text = "Valor Hora:";

            // Form1
            this.ClientSize = new System.Drawing.Size(670, 350);
            this.Controls.Add(this.txtPlaca);
            this.Controls.Add(this.cmbTipoVeiculo);
            this.Controls.Add(this.txtValorHora);
            this.Controls.Add(this.btnRegistrarEntrada);
            this.Controls.Add(this.btnRegistrarSaida);
            this.Controls.Add(this.dgvVeiculos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Name = "Form1";
            this.Text = "Sistema de Estacionamento";
            ((System.ComponentModel.ISupportInitialize)(this.dgvVeiculos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
