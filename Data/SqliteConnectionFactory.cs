using Microsoft.Data.Sqlite;

namespace ConnectFour.Data;

public static class SqliteConnectionFactory
{
    public static string CreateConnectionString(string dbPath)
    {
        return new SqliteConnectionStringBuilder
        {
            DataSource = dbPath,
            Mode = SqliteOpenMode.ReadWriteCreate,
            Cache = SqliteCacheMode.Default
        }.ToString();
    }
}
