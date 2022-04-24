using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQL_Decryptor
{
    class Registry
    {
        public T GetValue<T>(RegistryKey Hive, string Path, string Key)
        {
            RegistryKey registryKey = Hive.OpenSubKey(Path);
            string[] a = registryKey.GetValueNames();

            if (registryKey != null)
            {
                return (T)registryKey.GetValue(Key);
            }

            return default(T);
        }
    }
}
