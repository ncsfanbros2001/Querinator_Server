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

        public static string DefaultServer()
        {
            string serverName = Dns.GetHostName();
            string serverTitle = serverName;
            RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
                if (instanceKey != null)
                {
                    foreach (var instanceName in instanceKey.GetValueNames())
                    {
                        serverTitle = $"{serverName}\\{instanceName}";
                        break;
                    }
                }
            }

            return serverTitle;
        }
    }
}
