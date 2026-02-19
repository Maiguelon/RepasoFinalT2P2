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
        
    }
    public void Add(Models.Manga manga)
    {
        
    }
    public void Update(int id)
    {
        
    }
    public void Delete(int id)
    {
        
    }
}