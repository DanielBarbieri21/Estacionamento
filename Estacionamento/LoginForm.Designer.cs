namespace Estacionamento
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblSenha;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.Button btnEntrar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblSenha = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.btnEntrar = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.Text = "Login - Estacionamento";
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitulo.Height = 50;
            this.lblTitulo.BackColor = System.Drawing.Color.SteelBlue;
            this.lblTitulo.ForeColor = System.Drawing.Color.White;

            // lblEmail
            this.lblEmail.Text = "E-mail:";
            this.lblEmail.Location = new System.Drawing.Point(40, 70);
            this.lblEmail.Size = new System.Drawing.Size(60, 20);

            // txtEmail
            this.txtEmail.Location = new System.Drawing.Point(110, 70);
            this.txtEmail.Size = new System.Drawing.Size(200, 23);

            // lblSenha
            this.lblSenha.Text = "Senha:";
            this.lblSenha.Location = new System.Drawing.Point(40, 110);
            this.lblSenha.Size = new System.Drawing.Size(60, 20);

            // txtSenha
            this.txtSenha.Location = new System.Drawing.Point(110, 110);
            this.txtSenha.Size = new System.Drawing.Size(200, 23);
            this.txtSenha.PasswordChar = '*';

            // btnEntrar
            this.btnEntrar.Text = "Entrar";
            this.btnEntrar.Location = new System.Drawing.Point(110, 150);
            this.btnEntrar.Size = new System.Drawing.Size(200, 35);
            this.btnEntrar.BackColor = System.Drawing.Color.LightGreen;
            this.btnEntrar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEntrar.Click += new System.EventHandler(this.btnEntrar_Click);

            // LoginForm
            this.ClientSize = new System.Drawing.Size(370, 220);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblSenha);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.btnEntrar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "LoginForm";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
