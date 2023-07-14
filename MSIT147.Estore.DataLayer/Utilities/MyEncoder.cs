using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MSIT147.Estore.DataLayer.Utilities
{
    static public class MyEncoder
    {
        static public string GetSaltedSha256(
            this string input,
            string salt = "+%zpark",
            string key = "ISpan147")
        {
            var message = input + salt;
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacSHA256 = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = hmacSHA256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
            }
        }
    }
}
