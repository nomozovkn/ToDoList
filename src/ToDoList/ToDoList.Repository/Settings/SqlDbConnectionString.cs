namespace ToDoList.Repository.Settings;


public class SqlDBConeectionString
{
    private string connectionString;

    public string ConnectionString
    {
        get { return connectionString; }
        set { connectionString = value; }
    }

    public SqlDBConeectionString(string connectionString)
    {
        ConnectionString = connectionString;
    }
}
