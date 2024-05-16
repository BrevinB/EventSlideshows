using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace DataAccessLibrary;

public interface ISqlDataAccess
{
    string ConnectionStringName { get; set; }
    Task<List<T>> LoadData<T, U>(string sql, U parameters);
    Task SaveData<T>(string sql, T parameters);
}

public class SqlDataAccess : ISqlDataAccess
{
     private readonly IConfiguration _config;

     public string ConnectionStringName { get; set; } = "Default";
     
     public SqlDataAccess(IConfiguration config)
     {
          _config = config;
     }

     public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
     {
         //var dbHost = "172.17.0.2";
         //var dbName = "GraduationSlideshowDB";
         //var dbPassword = "Sparty1468";
         //var connectionString = $"$Data Source{{dbHost}};Initial Catalog={{dbName}};User ID=sa;Password={dbPassword}";
         string connectionString = _config.GetConnectionString(ConnectionStringName);

         using (IDbConnection connection = new SqlConnection(connectionString))
         {
             var data = await connection.QueryAsync<T>(sql, parameters);

             return data.ToList();
         }
     }

     public async Task SaveData<T>(string sql, T parameters)
     {
         string connectionString = _config.GetConnectionString(ConnectionStringName);

         using (IDbConnection connection = new SqlConnection(connectionString))
         {
             await connection.ExecuteAsync(sql, parameters);
         }
     }
}