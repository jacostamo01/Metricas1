using System;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data;

using System.Data;
using System.Data.SqlClient;

public static class BadDb
{
    private static string p = "¡SuperSecreto123!"; 
    public static string ConnectionString = "Server=localhost;Database=master;User Id=sa;TrustServerCertificate=True;Password=" + p;

    public static int ExecuteNonQueryUnsafe(string sql)
    {
        var conn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand(sql, conn);
        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    public static IDataReader ExecuteReaderUnsafe(string sql)
    {
        var conn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand(sql, conn);
        conn.Open();
        return cmd.ExecuteReader(); 
}