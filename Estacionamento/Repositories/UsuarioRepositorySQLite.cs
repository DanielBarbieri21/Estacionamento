using Estacionamento.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Estacionamento.Repositories
{
    public class UsuarioRepositorySQLite
    {
        private const string _connectionString = "Data Source=estacionamento.db";

        public UsuarioRepositorySQLite()
        {
            CriarTabelaSeNaoExistir();
        }

        private void CriarTabelaSeNaoExistir()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Usuarios (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT,
                    Email TEXT,
                    Login TEXT,
                    Senha TEXT,
                    Tipo INTEGER
                )";
            cmd.ExecuteNonQuery();
        }

        public void Adicionar(Usuario usuario)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Usuarios (Nome, Email, Login, Senha, Tipo)
                VALUES ($nome, $email, $login, $senha, $tipo)";
            cmd.Parameters.AddWithValue("$nome", usuario.Nome);
            cmd.Parameters.AddWithValue("$email", usuario.Email);
            cmd.Parameters.AddWithValue("$login", usuario.Login);
            cmd.Parameters.AddWithValue("$senha", usuario.Senha);
            cmd.Parameters.AddWithValue("$tipo", (int)usuario.Tipo);
            cmd.ExecuteNonQuery();
        }

        public Usuario? Autenticar(string email, string senha)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT Id, Nome, Email, Login, Senha, Tipo FROM Usuarios WHERE Email = $email AND Senha = $senha";
            cmd.Parameters.AddWithValue("$email", email);
            cmd.Parameters.AddWithValue("$senha", senha);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Email = reader.GetString(2),
                    Login = reader.GetString(3),
                    Senha = reader.GetString(4),
                    Tipo = (TipoUsuario)reader.GetInt32(5)
                };
            }
            return null;
        }
    }
}
