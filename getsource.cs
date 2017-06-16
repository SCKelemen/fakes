using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecurityFramework.Streams
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

        public void GetSource(Stream  sourceStream)
        {
            source = sourceStream;
        }

        public void GetDestination(Stream destinationStream)
        {
            destination = destinationStream;
        } 

        public void Encrypt(Stream keyStream, byte[] iv)
        {
            
        }

        public void Decrypt()
        {
            
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

    class DECCryptor : Cryptor
    {
        public void Encrypt(Stream keyStream, byte[] iv)
        {
            
        }

        public void Decrypt(Stream keyStream, byte[] iv)
        {
            
        }
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
                    cryptor.GetDestination(File.Create(destination));
                }
                else
                {
                    cryptor.GetDestination(File.OpenWrite(destination));
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
                cryptor.GetSource(File.OpenRead(source));
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
            
        }
        
    }

}
