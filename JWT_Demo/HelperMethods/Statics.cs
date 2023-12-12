using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System.Net;

namespace JWT_Demo.HelperMethods
{
    public static class Statics
    {
        public const string AdminRole = "admin";
        public const string CustomerRole = "customer";

        public const string QueryDbConnectionName = "DB_To_Query_Connection";
        public const string OperatorDbConnectionName = "DB_To_Operate_Connection";

        public static string CurrentServer = "";
        public static string CurrentDatabase = "";

        public static List<string> DefaultServer()
        {
            List<string> servers = new List<string>();

            string serverName = Dns.GetHostName();
            RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
                if (instanceKey != null)
                {
                    foreach (var instanceName in instanceKey.GetValueNames())
                    {
                        servers.Add($"{serverName}\\{instanceName}");
                    }
                }
            }

            return servers;
        }

        public static List<string> DefaultDatabases() 
        {
            List<string> databases = new List<string>();

            string retrieveDbQuery = $"SELECT name FROM sys.databases Where name != 'Querinator';";

            string dbName;

            if (Environment.GetEnvironmentVariable(QueryDbConnectionName) == null)
            {
                dbName = OperatorDbConnectionName;
            }
            else
            {
                dbName = QueryDbConnectionName;
            }

            using (var connection = new SqlConnection(
                    Environment.GetEnvironmentVariable(dbName)))
            {
                databases = (List<string>)connection.Query<string>(retrieveDbQuery);
            }

            return databases;
        }

        public static string WindowsAuthenticationCS(string serverName, string databaseName)
        {
            return $"Server={serverName};Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True";
        }

        public static string SqlServerCS(string serverName, string databaseName, string username, string password)
        {
            return $"Server={serverName};Database={databaseName};User ID={username};Password={password};" +
                $"TrustServerCertificate=True";
        }
    }
}
