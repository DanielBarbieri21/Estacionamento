using Estacionamento.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Estacionamento.Repositories
{
    public class VeiculoRepositorySQLite
    {
        private const string _connectionString = "Data Source=estacionamento.db";

        public VeiculoRepositorySQLite()
        {
            CriarTabelaSeNaoExistir();
            MigrarEsquemaSeNecessario();
        }

        private void CriarTabelaSeNaoExistir()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Veiculos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Placa TEXT,
                    Tipo INTEGER,
                    Entrada TEXT,
                    Saida TEXT,
                    ValorHora REAL
                )";
            cmd.ExecuteNonQuery();
        }

        private void MigrarEsquemaSeNecessario()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            // Verifica se a coluna Id existe; se não, migrar
            var checkCmd = conn.CreateCommand();
            checkCmd.CommandText = "PRAGMA table_info(Veiculos);";
            using var reader = checkCmd.ExecuteReader();
            var possuiId = false;
            while (reader.Read())
            {
                var nomeColuna = reader.GetString(1);
                if (string.Equals(nomeColuna, "Id", StringComparison.OrdinalIgnoreCase))
                {
                    possuiId = true;
                    break;
                }
            }
            reader.Close();

            if (!possuiId)
            {
                using var tx = conn.BeginTransaction();
                var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
                // Cria nova tabela com Id autoincrement e sem PK em Placa
                cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Veiculos_new (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Placa TEXT,
    Tipo INTEGER,
    Entrada TEXT,
    Saida TEXT,
    ValorHora REAL
);
INSERT INTO Veiculos_new (Placa, Tipo, Entrada, Saida, ValorHora)
    SELECT Placa, Tipo, Entrada, Saida, ValorHora FROM Veiculos;
DROP TABLE Veiculos;
ALTER TABLE Veiculos_new RENAME TO Veiculos;";
                cmd.ExecuteNonQuery();
                tx.Commit();
            }
        }

        public void Adicionar(Veiculo veiculo)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Veiculos (Placa, Tipo, Entrada, Saida, ValorHora)
                VALUES ($placa, $tipo, $entrada, $saida, $valorHora)";
            cmd.Parameters.AddWithValue("$placa", veiculo.Placa);
            cmd.Parameters.AddWithValue("$tipo", (int)veiculo.Tipo);
            cmd.Parameters.AddWithValue("$entrada", veiculo.Entrada.ToString("o"));
            if (veiculo.Saida == null)
                cmd.Parameters.AddWithValue("$saida", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("$saida", veiculo.Saida.Value.ToString("o"));
            cmd.Parameters.AddWithValue("$valorHora", veiculo.ValorHora);
            cmd.ExecuteNonQuery();
        }

        public void Atualizar(Veiculo veiculo)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Veiculos SET Saida = $saida WHERE Placa = $placa";
            cmd.Parameters.AddWithValue("$placa", veiculo.Placa);
            cmd.Parameters.AddWithValue("$saida", veiculo.Saida?.ToString("o"));
            cmd.ExecuteNonQuery();
        }

        public void AtualizarDados(string placa, TipoVeiculo tipo, decimal valorHora)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Veiculos SET Tipo = $tipo, ValorHora = $valorHora WHERE Placa = $placa AND Saida IS NULL";
            cmd.Parameters.AddWithValue("$placa", placa);
            cmd.Parameters.AddWithValue("$tipo", (int)tipo);
            cmd.Parameters.AddWithValue("$valorHora", valorHora);
            cmd.ExecuteNonQuery();
        }

        public void RemoverAtivoPorPlaca(string placa)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Veiculos WHERE Placa = $placa AND Saida IS NULL";
            cmd.Parameters.AddWithValue("$placa", placa);
            cmd.ExecuteNonQuery();
        }

        public Veiculo? ObterPorPlaca(string placa)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT Placa, Tipo, Entrada, Saida, ValorHora FROM Veiculos WHERE Placa = $placa AND (Saida IS NULL OR Saida = '')";
            cmd.Parameters.AddWithValue("$placa", placa);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Veiculo
                {
                    Placa = reader.GetString(0),
                    Tipo = (TipoVeiculo)reader.GetInt32(1),
                    Entrada = DateTime.Parse(reader.GetString(2)),
                    Saida = reader.IsDBNull(3) ? null : DateTime.Parse(reader.GetString(3)),
                    ValorHora = reader.GetDecimal(4)
                };
            }
            return null;
        }

        public List<Veiculo> ObterTodos()
        {
            var lista = new List<Veiculo>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT Placa, Tipo, Entrada, Saida, ValorHora FROM Veiculos";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Veiculo
                {
                    Placa = reader.GetString(0),
                    Tipo = (TipoVeiculo)reader.GetInt32(1),
                    Entrada = DateTime.Parse(reader.GetString(2)),
                    Saida = reader.IsDBNull(3) ? null : DateTime.Parse(reader.GetString(3)),
                    ValorHora = reader.GetDecimal(4)
                });
            }
            return lista;
        }

        public void RemoverFinalizadoPorPlaca(string placa)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Veiculos WHERE Placa = $placa AND Saida IS NOT NULL";
            cmd.Parameters.AddWithValue("$placa", placa);
            cmd.ExecuteNonQuery();
        }
    }
}
