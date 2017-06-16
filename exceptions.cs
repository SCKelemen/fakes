using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecurityFramework.Cipher
{
    
    public class DeprecatedCipherException : CryptographicException
    {
        public DeprecatedCipherException()
        {
            
        }

        public DeprecatedCipherException(string message)
        {
            
        }

        public DeprecatedCipherException(int hr)
        {
            
        }

        public DeprecatedCipherException(string format, string insert)
        {
            
        }

        public DeprecatedCipherException(string message, Exception inner)
        {
            
        }
    }
    public class DeprecatedHashException : CryptographicException
    {
        public DeprecatedHashException()
        {
            
        }

        public DeprecatedHashException(string message)
        {
            
        }

        public DeprecatedHashException(int hr)
        {
            
        }

        public DeprecatedHashException(string format, string insert)
        {
            
        }

        public DeprecatedHashException(string message, Exception inner)
        {
            
        }

    }
}
