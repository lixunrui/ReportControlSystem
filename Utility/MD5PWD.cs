using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ReportControlSystem
{
    internal static class MD5PWD
    {
        internal static String MD5HashPassword(String password)
        {
            MD5 md5Hasher = MD5.Create();
            
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(password));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++ )
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
