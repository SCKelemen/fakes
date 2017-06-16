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

    class RijndaelFileCryptor : RijndaelCryptor, IFileEncryptor
    {
        public void EncryptFile(string filename)
        {
            
        }

        public void EncryptFile(Uri uri)
        {
            
        }

        public void EncryptFile(DirectoryInfo dir)
        {
            
        }

        public void EncryptFile(FileInfo file)
        {
            
        }
    }

    interface IFileEncryptor
    {
        void EncryptFile(string filename);
        void EncryptFile(Uri uri);
        void EncryptFile(DirectoryInfo dir);
        void EncryptFile(FileInfo file);
    }

    class RijndaelCryptor : IStreamDecryptor, IStreamEncryptor
    {
        public Stream Encrypt(Stream sourceStream, Stream destinationStream, Stream keyStream, byte[] IV)
        {
            return destinationStream;
        }

        public Stream Decrypt(Stream sourceStream, Stream destinationStream, Stream keyStream, byte[] IV)
        {
            return sourceStream;
        }
    }


    interface IStreamEncryptor
    {
        Stream Encrypt(Stream source, Stream destination, Stream key, byte[] IV);

    }

    interface IStreamDecryptor
    {
        Stream Decrypt(Stream source, Stream destination, Stream key, byte[] IV);

    }
}
