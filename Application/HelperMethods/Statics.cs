using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System.Data;
using System.Net;

namespace Application.HelperMethods
{
    public static class Statics
    {
        public const string AdminRole = "admin";
        public const string UserRole = "user";

        public const string QueryDbConnectionName = "DB_To_Query_Connection";
        public const string OperatorDbConnectionName = "DB_To_Operate_Connection";

        public const string OperatorServerName = "LAPTOP-NATTQ2BG\\MSSQLSERVER01";
        public const string OperatorDbName = "Querinator";

        public static List<string> DefaultServers()
        {
            List<string> servers = new();

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
            List<string> databases = new();

            string connectionString = $"Server={DefaultServers().First()};Trusted_Connection=True;" +
                    $"TrustServerCertificate=True;";

            using SqlConnection connection = new(connectionString);

            try
            {
                connection.Open();
                DataTable databasesList = connection.GetSchema("Databases");

                foreach (DataRow database in databasesList.Rows)
                {
                    string databaseName = database.Field<string>("database_name");
                    if (databaseName != OperatorDbName || databaseName != "master" || databaseName != "tempdb"
                       || databaseName != "model" || databaseName != "msdb")
                    {
                        databases.Add(databaseName);
                    }
                }

                connection.Close();
                return databases;
            }
            catch
            {
                connection.Close();
                return new List<string>();
            }
        }

        public static string WindowsAuthCS(string serverName, string databaseName)
        {
            return $"Server={serverName};Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True";
        }

        public static string SqlServerCS(string serverName, string databaseName, string username, string password)
        {
            return $"Server={serverName};Database={databaseName};User ID={username};Password={password};" +
                $"TrustServerCertificate=True;";
        }
    }
}
