using System;
using System.Data.SqlClient;

public sealed class DatabaseConnection
{
    private static readonly Lazy<DatabaseConnection> lazy =
        new Lazy<DatabaseConnection>(() => new DatabaseConnection());

    public static DatabaseConnection Instance { get { return lazy.Value; } }

    private readonly SqlConnection connection;

    private DatabaseConnection()
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = "188.119.112.190";
        builder.InitialCatalog = "EquipRentalPointDB";
        builder.UserID = "erp_admin";
        builder.Password = "admin";
        connection = new SqlConnection(builder.ConnectionString);
        connection.ConnectionString = builder.ConnectionString;
    }

    public SqlConnection GetConnection()
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = "188.119.112.190";
        builder.InitialCatalog = "EquipRentalPointDB";
        builder.UserID = "erp_admin";
        builder.Password = "admin";

        SqlConnection connection = new SqlConnection(builder.ConnectionString);

        return connection;
    }
}

