using System.Security.Cryptography;
using System.Text;

namespace helloworld.service
{
    public class Md5
    {
        public string HashPassword(string password, string salt)
        {
            // Create an MD5 hash of the supplied password using the supplied salt as well.
            string sourceText = salt + password;
            ASCIIEncoding asciiEnc = new ASCIIEncoding();
            string hash = null;
            byte[] byteSourceText = asciiEnc.GetBytes(sourceText);
            MD5CryptoServiceProvider md5Hash = new MD5CryptoServiceProvider();
            byte[] byteHash = md5Hash.ComputeHash(byteSourceText);
            foreach (byte b in byteHash) {
                hash += b.ToString("x2");
            }

            // Return the hashed password
            return hash;
        }
    }
}