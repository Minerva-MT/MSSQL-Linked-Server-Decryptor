using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SQL_Decryptor
{
    class Program
    {
        /*
         * 1. Get Installed Instances from Registry
         * 2. Connect to Database using DAC
         * 3. Get Encrypted Service Key from Database
         * 4. Get SQL Entropy from the Registry
         * 5. Decrypt Service Key
         * 6. Get Encrypted Passwords and IV from Database
         * 7. Decrypt Passwords
         */

        static void Main(string[] args)
        {
            
            Console.WriteLine("+--------------------------------------------------------------------------------+");
            Console.WriteLine("|              MSSQL Linked Server Password Decryptor - Minerva MT               |");
            Console.WriteLine("|                          andrew.borg@minerva.com.mt                            |");
            Console.WriteLine("+--------------------------------------------------------------------------------+\r\n");

            try
            {
                Decryptor SQLDecryptor = new Decryptor();
                SQLDecryptor.DecryptPasswords();
            }
            catch (Exception)
            {
                Console.WriteLine("Error Occured, Exiting.");
            }
        }
    }
}
