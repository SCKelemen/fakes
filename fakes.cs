using System;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityFramework
{
    

    public class Fakes
    {
        public static Stream Encrypt(Stream source, Stream destination, Stream key, byte[] IV)
        {
            using (Rijndael rjn = Rijndael.Create())
            {
                key.Read(rjn.Key, 0, rjn.KeySize);
                rjn.IV = IV;

                ICryptoTransform encryptor = rjn.CreateEncryptor(rjn.Key, rjn.IV);

                using (CryptoStream cse = new CryptoStream(destination, encryptor, CryptoStreamMode.Write))
                {
                    return cse;
                }


            }

                
        }
        
    }

}
