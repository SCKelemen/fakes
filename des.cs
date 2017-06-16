using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace SecurityFramework.Cryptorz
{
    interface ICryptable
    {
        void Encrypt();
        void Encrypt(Stream keyStream, byte[] iv);
        void Decrypt();

    }

    abstract class CryptorBase : ICryptable, IDisposable
    {
        private Stream _source, _destination;

        public CryptorBase(Stream source, Stream destination)
        {
            _source = source;
            _destination = destination;
        }
         
        public Stream Source
        {
            get { return _source; }
            
        }

        public Stream Destination
        {
            get { return _destination; }
        }


        public void Encrypt(Stream keyStream, byte[] iv)
        {
            
        }

        public void Decrypt()
        {
            //satisfy interface
        }

        public void Encrypt()
        {
            //satisfy interface   
        }

        

        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposed) { return;}
            if (disposing) {  handle.Dispose();}
            disposed = true;
        }
    }

    class RijndaelCryptor : CryptorBase
    {
       public void Encrypt(Stream keyStream, byte[] iv)
        {
            int keyLength;
            try
            {
                keyLength = Convert.ToInt32(keyStream.Length - 1);
            }
            catch (OverflowException ex)
            {
                throw new InvalidKeySizeException();
            }

            Rijndael rijndael = Rijndael.Create();

            //ensure that the key is a valid key length for Rijndael
            if (!rijndael.ValidKeySize(keyLength))
            {
                throw new InvalidKeySizeException();
            }
            //set the Rijndael key as keystream[0, keyStream.Length] the full stream
            keyStream.Read(rijndael.Key, 0, keyLength);

            //set the IV
            rijndael.IV = iv;

            //create a transform
            ICryptoTransform cryptoTransform = rijndael.CreateEncryptor();

            //generate xor stream with the destination, the encryptor transform, and this is a write operation
            CryptoStream cryptoStream = new CryptoStream(Destination, cryptoTransform, CryptoStreamMode.Write);

            //write the stream
            byte[] buffer = new byte[Source.Length - 1];
            Source.Read(buffer, 0, buffer.Length);
            cryptoStream.Write(buffer, 0, buffer.Length);
        }

        public void Decrypt(Stream keyStream, byte[] iv)
        {
            
        }   
    }

    class DESCryptor : CryptorBase
    {
        public void Encrypt(Stream keyStream, byte[] iv)
        {
            int keyLength;
            try
            {
                keyLength = Convert.ToInt32(keyStream.Length - 1);
            }
            catch (OverflowException ex)
            {
                throw new InvalidKeySizeException();
            }

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

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
            CryptoStream cryptoStream = new CryptoStream(Destination, cryptoTransform, CryptoStreamMode.Write);

            //write the stream
            byte[] buffer = new byte[Source.Length - 1];
            Source.Read(buffer, 0, buffer.Length);
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
                    cryptor.Destination =  File.Create(destination);
                }
                else
                {
                    cryptor.Destination = File.OpenWrite(destination);
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
                cryptor.Source = File.OpenRead(source);
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
                    cryptor.Destination = File.Create(destination);
                }
                else
                {
                    cryptor.Destination = File.OpenWrite(destination);
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
                cryptor.Source = File.OpenRead(source);
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
