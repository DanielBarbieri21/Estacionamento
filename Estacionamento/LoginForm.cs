using Estacionamento.Repositories;
using System;
using System.Windows.Forms;

namespace Estacionamento
{
    public partial class LoginForm : Form
    {
        private readonly UsuarioRepositorySQLite _usuarioRepo = new UsuarioRepositorySQLite();

        public LoginForm()
        {
            InitializeComponent();
            AdicionarUsuarioAdminPadrao();
        }

        private void AdicionarUsuarioAdminPadrao()
        {
            // Verifica se já existe admin
            var repo = new UsuarioRepositorySQLite();
            var usuario = repo.Autenticar("admin", "246895");
            if (usuario == null)
            {
                repo.Adicionar(new Models.Usuario
                {
                    Nome = "Administrador",
                    Email = "admin",
                    Login = "admin",
                    Senha = "246895",
                    Tipo = Models.TipoUsuario.Admin
                });
            }
    // ...existing code...
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            var email = txtEmail.Text.Trim();
            var senha = txtSenha.Text;
            var usuario = _usuarioRepo.Autenticar(email, senha);
            if (usuario != null)
            {
                // Usuário autenticado, abrir tela principal
                this.Hide();
                var formPrincipal = new Form1();
                formPrincipal.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("E-mail ou senha inválidos.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
