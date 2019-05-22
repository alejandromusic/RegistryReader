using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ProgramModel
    {
        public string guid { get; set; }
        public string name { get; set; }
        public string type { get; set; }

        static string path1 = "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
        static string path2 = "Software\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall";

        public static void findName(string name, List<ProgramModel> data)
        {
            // navigate the register
            RegistryKey rk1 = Registry.LocalMachine.OpenSubKey(path1);
            RegistryKey rk2 = Registry.LocalMachine.OpenSubKey(path2);

            PrintKeysWithName(name, rk1, "32bit", data);
            PrintKeysWithName(name, rk1, "64bit", data);
        }

        public static void findGUID(string guid, List<ProgramModel> data)
        {
            

            // navigate the register
            RegistryKey rk1 = Registry.LocalMachine.OpenSubKey(path1);
            RegistryKey rk2 = Registry.LocalMachine.OpenSubKey(path2);

            PrintKeysWithGUID(guid, rk1, "32bit", data);
            PrintKeysWithGUID(guid, rk1, "64bit", data);
            
        }

        public static void sort(string param)
        {
            if (param == "name")
            {

            }
        }

        private static void PrintKeysWithName(string name, RegistryKey rk, string type, List<ProgramModel> data)
        {

            // for each guid
            foreach (var subKey in rk.GetSubKeyNames())
            {

                RegistryKey productKey = rk.OpenSubKey(subKey);

                // ensure key exists
                if (productKey == null)
                    continue;

                // get product name
                string productName = Convert.ToString(productKey.GetValue("DisplayName"));

                // ensure product name is not empty
                if (productName == "")
                    continue;

                // if name, print
                string lower = productName.ToLower();

                if (!lower.Contains(name.ToLower()))
                    continue;

                // write line
                data.Add( new ProgramModel { guid=subKey, name=productName, type=type }  );
            }



        }

        private static void PrintKeysWithGUID(string guid, RegistryKey rk, string type, List<ProgramModel> data)
        {
            // extract number of char
            int n = guid.Length;


            // for each guid
            foreach (var subKey in rk.GetSubKeyNames())
            {
                if (subKey.Length <= n)
                    continue;

                // if first char of guid is a curly brace, skip it
                int beginAtChar = (String.Compare(subKey.Substring(0, 1), "{") == 0) ?
                    1 : 0;

                // build substring from registry to compare
                string registrySubstr = subKey.Substring(beginAtChar, n).ToLower();

                // if guid matches the registry subKey
                if (String.Compare(registrySubstr, guid.ToLower() ) != 0)
                    continue;

                RegistryKey productKey = rk.OpenSubKey(subKey);

                // ensure key exists
                if (productKey == null)
                    continue;

                // get product name
                string productName = Convert.ToString(productKey.GetValue("DisplayName"));

                // ensure product name is not empty
                if (productName == "")
                    continue;

                // write line
                data.Add(new ProgramModel { guid = subKey, name = productName, type = type });
            }

        }

    }
}