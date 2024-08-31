using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace BackEnd.Helper
{
    public static class StringExtensions
    {
        public static string Proper(this string text)
        {
            return string.Join(" ", Regex.Split(text, @"(?<!^)(?=[A-Z])"));
        }
        public static string EncodePassword(this string password)
        {
            using (var md5 = new SHA512CryptoServiceProvider())
            {
                //compute hash from the bytes of text
                md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                //get hash result after compute it
                var result = md5.Hash;
                var strBuilder = new StringBuilder();
                for (var i = 0; i < result.Length; i++)
                {
                    //change it into 2 hexadecimal digits
                    //for each byte
                    strBuilder.Append(result[i].ToString("x2"));
                }
                return strBuilder.ToString();
            }
        }

    }
}
