using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System.Data;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Application.HelperMethods
{
    public class Statics
    {
        public const string AdminRole = "admin";
        public const string UserRole = "user";

        public static string OperatorServerName = Dns.GetHostName() + "\\QUERINATOR";
        public const string OperatorDbName = "Querinator";

        private const string Key = "7XIh3u9xDudo7xm1";

        public static List<string> DefaultServers()
        {
            List<string> servers = new();

            string serverName = Dns.GetHostName();
            // servers.Add(serverName);

            RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
                
                if (instanceKey != null)
                {
                    foreach (var instanceName in instanceKey.GetValueNames())
                    {
                        if (instanceName != OperatorDbName.ToUpper())
                        {
                            servers.Add($"{serverName}\\{instanceName}");
                        }
                    }
                }
            }

            return servers;
        }

        public static List<string> DefaultDatabases(string server) 
        {
            List<string> databases = new();

            string connectionString = $"Server={server};Trusted_Connection=True;TrustServerCertificate=True;";

            using SqlConnection connection = new(connectionString);

            try
            {
                connection.Open();
                DataTable databasesList = connection.GetSchema("Databases");

                foreach (DataRow database in databasesList.Rows)
                {
                    string databaseName = database.Field<string>("database_name");
                    if (databaseName != OperatorDbName && databaseName != "master" && databaseName != "tempdb"
                       && databaseName != "model" && databaseName != "msdb")
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

        public static string Encrypt(string text)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = new byte[16];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string Decrypt(string encryptedText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = new byte[16];

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
