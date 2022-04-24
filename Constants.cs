using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQL_Decryptor
{
    static class Constants
    {
        public static string Registry_Entropy = @"SOFTWARE\Microsoft\Microsoft SQL Server\{0}\Security\";
        public static string Registry_Instances = @"SOFTWARE\Microsoft\Microsoft SQL Server";
        public static string SQL_INSTANCE = @"Server=ADMIN:LOCALHOST\{0};Trusted_Connection=True";
        public static string SQL_SERVICE_KEY = @"
            SELECT SUBSTRING(crypt_property,9,len(crypt_property)-8) 
            FROM sys.key_encryptions 
            WHERE key_id=102 and (thumbprint=0x03 or thumbprint=0x0300000001)";
        public static string SQL_ENCRYPTED_PASSWORDS = @"
            SELECT 
                sysservers.srvname                                                  Server, 
                syslnklgns.name                                                     Username, 
                SUBSTRING(syslnklgns.pwdhash,5,{0})                                 IV,
                SUBSTRING(syslnklgns.pwdhash, {1}, len(syslnklgns.pwdhash)-{2})     Hash
            FROM master.sys.syslnklgns 
            INNER JOIN master.sys.sysservers on syslnklgns.srvid=sysservers.srvid 
            WHERE len(pwdhash)>0";
        public static string SEPERATOR = "\r\n--------------------------------------------------------------------------------\r\n";
    }
}
