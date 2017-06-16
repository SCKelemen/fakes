using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecurityFramework.Cryptorz
{
    interface ICryptable
    {
        void GetSource();
        void GetDestination();
        void GetKey();
        void GetIV();
        void Encrypt();
        void Encrypt(Stream keyStream, byte[] iv);
        void Decrypt();

    }

    abstract class Cryptor
    {
        private Stream source, destination;

        public void SetSource(Stream  sourceStream)
        {
            source = sourceStream;
        }

        public void SetDestination(Stream destinationStream)
        {
            destination = destinationStream;
        } 

        public void Encrypt(Stream keyStream, byte[] iv)
        {
            
        }

        public void Decrypt()
        {
            
        }

        protected Stream getSource()
        {
            return source;
        }

        protected Stream getDestination()
        {
            return destination;
        }
    }

    class RijndaelCryptor : Cryptor
    {

        public void Encrypt(Stream keyStream, byte[] iv)
        {
            
        }

        public void Decrypt(Stream keySteStream, byte[] iv)
        {
            
        }   
    }

    class DESCryptor : Cryptor
    {
        public void Encrypt(Stream keyStream, byte[] iv)
        {
            
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            int keyLength;

            //get keystream legnth as int
            if (!int.TryParse(keyStream.Length.ToString(), out keyLength))
            {
                //cast to int instead of string

                throw new InvalidKeySizeException();
            }
            //ensure that the key is a valid key length for DES

            if (!DES.ValidKeySize(keyLength))
            {
                throw new InvalidKeySizeException();
            }
            //set the DES key as keystream[0, keyStream.Length] the full stream
            keyStream.Read(DES.Key, 0, keyLength);

            //set the IV
            DES.IV = iv;

            //create a transform
            ICryptoTransform cryptoTransform = DES.CreateEncryptor();

            //generate xor stream with the destination, the encryptor transform, and this is a write operation
            CryptoStream cryptoStream = new CryptoStream(getDestination(), cryptoTransform, CryptoStreamMode.Write);

            //write the stream
            byte[] buffer = new byte[getSource().Length - 1];
            getSource().Read(buffer, 0, buffer.Length);
            cryptoStream.Write(buffer, 0, buffer.Length);


        }

        public void Decrypt(Stream keyStream, byte[] iv)
        {
            
        }
    }

    public class InvalidKeySizeException : CryptographicException
    {
        
    }

    class test
    {

        void EncryptWithRijndael()
        {


            string source = Path.Combine(Environment.SystemDirectory, "rijnSource.txt");
            if (!File.Exists(source))
            {
                throw new FileNotFoundException();
            }
            var cryptor = new RijndaelCryptor();
            
            string destination = Path.Combine(Environment.SystemDirectory, "rijnDestination.txt");
            try
            {
                if (!File.Exists(destination))
                {
                    cryptor.SetDestination(File.Create(destination));
                }
                else
                {
                    cryptor.SetDestination(File.OpenWrite(destination));
                }
            }
            catch (FileNotFoundException ex)
            {
                //recreate file
            }
            catch (FileLoadException ex)
            {
                //thats not good
            }
            catch (IOException ex)
            {
                //thats really not good
            }

            try
            {
                cryptor.SetSource(File.OpenRead(source));
            }
            catch (FileNotFoundException ex)
            {
                //uh-oh, someone deleted the file before we could encrypt it!
            }
            catch (FileLoadException ex)
            {
                //thats not good
            }
            catch (IOException ex)
            {
                //thats really not good
            }


            //for now I will just create these objects
            //in theory, they'll be passed in
            var r = Rijndael.Create();
            r.GenerateKey();
            r.GenerateIV();
            cryptor.Encrypt(new MemoryStream(r.Key), r.IV);
           
        }

        void EncryptWithDES()
        {
            string source = Path.Combine(Environment.SystemDirectory, "desSource.txt");
            if (!File.Exists(source))
            {
                throw new FileNotFoundException();
            }
            var cryptor = new DESCryptor();

            string destination = Path.Combine(Environment.SystemDirectory, "desDestination.txt");
            try
            {
                if (!File.Exists(destination))
                {
                    cryptor.SetDestination(File.Create(destination));
                }
                else
                {
                    cryptor.SetDestination(File.OpenWrite(destination));
                }
            }
            catch (FileNotFoundException ex)
            {
                //recreate file
            }
            catch (FileLoadException ex)
            {
                //thats not good
            }
            catch (IOException ex)
            {
                //thats really not good
            }

            try
            {
                cryptor.SetSource(File.OpenRead(source));
            }
            catch (FileNotFoundException ex)
            {
                //uh-oh, someone deleted the file before we could encrypt it!
            }
            catch (FileLoadException ex)
            {
                //thats not good
            }
            catch (IOException ex)
            {
                //thats really not good
            }


            //for now I will just create these objects
            //in theory, they'll be passed in
            var r = DES.Create();
            r.GenerateKey();
            r.GenerateIV();
            cryptor.Encrypt(new MemoryStream(r.Key), r.IV);
        }
        
    }

}
