using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityFramework.Sanitization
{
    interface ISanitizable
    {
        string Sanitize(string url);
    }

    class URLSanitizer : ISanitizable
    {
        public string Sanitize(string url)
        {
            return Uri.EscapeUriString(url).ToString();
        }
    }
}
