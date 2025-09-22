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
        }

        private void CriarTabelaSeNaoExistir()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Veiculos (
                    Placa TEXT PRIMARY KEY,
                    Tipo INTEGER,
                    Entrada TEXT,
                    Saida TEXT,
                    ValorHora REAL
                )";
            cmd.ExecuteNonQuery();
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

        public Veiculo? ObterPorPlaca(string placa)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT Placa, Tipo, Entrada, Saida, ValorHora FROM Veiculos WHERE Placa = $placa AND Saida IS NULL";
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
    }
}
