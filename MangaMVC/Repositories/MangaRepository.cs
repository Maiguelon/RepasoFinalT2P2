using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using TiendaManga.Models;
using TiendaManga.Interfaces;

namespace TiendaManga.Repositorios;

public class MangaRepository : IMangaRepository
{
    private readonly string CadenaConexion = "Data Source=./DB/TiendaManga.db";

    public MangaRepository()
    {
    }

    // Crud
    public List<Models.Manga> GetAll()
    {
        var lista = new List<Models.Manga>();
        const string sql = "SELECT Id, Titulo, TomosPublicados, Demografia FROM Manga ORDER BY Titulo ASC";

        using var conexion = new SqliteConnection(CadenaConexion);
        conexion.Open();

        using var comando = new SqliteCommand(sql, conexion);
        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            // Obtengo la cat como string, luego la parseo a enum
            string demografiaString = lector.GetString(3);
            Demografia dem;
            Enum.TryParse<Demografia>(demografiaString, true, out dem);

            var manga = new Models.Manga
            {
                Id = lector.GetInt32(0),
                Titulo = lector.GetString(1),
                TomosPublicados = lector.GetInt32(2),
                Demografia = dem
            };
            lista.Add(manga);
        }
        return lista;
    }

    public Models.Manga GetById(int id)
    {
        Models.Manga manga = null;

        using (var connection = new SqliteConnection(CadenaConexion))
        {
            connection.Open();
            string sql = @"SELECT Id, Titulo, TomosPublicados, Demografia
                FROM Peliculas
                WHERE Id = @id";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) // si lee algo
                    {
                        string demografiaString = reader.GetString(3);
                        Demografia dem;
                        Enum.TryParse<Demografia>(demografiaString, true, out dem);

                        manga = new Models.Manga()
                        {
                            Id = reader.GetInt32(0),
                            Titulo = reader.GetString(1),
                            TomosPublicados = reader.GetInt32(2),
                            Demografia = dem,
                        };
                    }
                }
            }
        }
        return manga;
    }

    public void Add(Models.Manga manga)
    {
        using var conexion = new SqliteConnection(CadenaConexion);
        conexion.Open();

        string sql = @"INSERT INTO Manga (Titulo, TomosPublicados, Demografia)
            VALUES(@tit, @tom, @dem)";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.AddWithValue("@tit", manga.Titulo);
        comando.Parameters.AddWithValue("@tom", manga.TomosPublicados);
        comando.Parameters.AddWithValue("@dem", manga.Demografia.ToString()); // para que se guarde con el nombre

        comando.ExecuteNonQuery();
    }
    public void Update(Manga manga)
    {
        using var conexion = new SqliteConnection(CadenaConexion);
        conexion.Open();

        string sql = @"UPDATE Manga
            SET Titulo = @tit, TomosPublicados = @tom, Demografia = @dem
            WHERE Id = @id";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.AddWithValue("@id", manga.Id);
        comando.Parameters.AddWithValue("@tit", manga.Titulo);
        comando.Parameters.AddWithValue("@tom", manga.TomosPublicados);
        comando.Parameters.AddWithValue("@dem", manga.Demografia.ToString());

        comando.ExecuteNonQuery();
    }
    public void Delete(int id)
    {
        using var conexion = new SqliteConnection(CadenaConexion);
        conexion.Open();

        string sql = @"DELETE FROM Manga
            WHERE Id = @id";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);

        comando.ExecuteNonQuery();
    }
}