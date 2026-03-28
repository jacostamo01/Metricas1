using System;
using System.Data;
using Microsoft.Data.SqlClient; // Recomendado usar Microsoft.Data.SqlClient en lugar de System.Data.SqlClient

namespace Infrastructure.Data;

public static class BadDb
{
    public static string ConnectionString { get; set; } = string.Empty;

    public static int ExecuteNonQueryUnsafe(string sql)
    {
        // Añadimos 'using' para asegurar que la conexión se cierre y libere memoria
        using var conn = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    public static IDataReader ExecuteReaderUnsafe(string sql)
    {
        // En Reader, no usamos 'using' en la conexión aquí porque el Reader la necesita abierta,
        // pero especificamos CommandBehavior.CloseConnection
        var conn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand(sql, conn);
        conn.Open();
        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }
}