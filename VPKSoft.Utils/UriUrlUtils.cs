﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPKSoft.Utils
{
    /// <summary>
    /// Some utils for validating and parsing uri's / url's.
    /// </summary>
    public static class UriUrlUtils
    {
        /// <summary>
        /// Checks if a valid HTTP(S) url was given to the <paramref name="url"/> string.
        /// 
        /// </summary>
        /// <remarks>
        /// Source code modified from Stack Overflow: https://stackoverflow.com/questions/7578857/how-to-check-whether-a-string-is-a-valid-http-url
        /// </remarks>
        /// <param name="url">The url which validity to check.</param>
        /// <param name="requireEndSlash">If the url requires an ending shlash character (/).</param>
        /// <returns>True if the given url was in valid format, otherwise false.</returns>
        public static bool ValidHttpUrl(string url, bool requireEndSlash = false)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps) &&
                   (requireEndSlash ? url.EndsWith("/") : true);
        }

        /// <summary>
        /// Creates a "wildcard" url to be used for e.g. netsh commamd.
        /// <para/>The format is http(s)://+:port/ect..
        /// </summary>
        /// <param name="url">Url string to make a wildcard url of.</param>
        /// <param name="stripEnd">Strips the end of the generated wildcard allowing the whole url space to be used. 
        /// <para/>E.g. http(s)://localhost:port/ect.. --> http(s)://+:port/</param>
        /// <returns>A wildcard url of the given url string.</returns>
        public static string MakeWildCardUrl(string url, bool stripEnd)
        {
            string retval = string.Empty;
            if (url.StartsWith("https://"))
            {
                retval += "https://+";
                url = url.Substring("https://".Length);
                if (url.IndexOf(":") > 0)
                {
                    retval += url.Substring(url.IndexOf(":"));
                }
                else
                {
                    retval += url.Substring(url.IndexOf("/"));
                }
            }
            else if (url.StartsWith("http://"))
            {
                retval += "http://+";
                url = url.Substring("http://".Length);
                if (url.IndexOf(":") > 0)
                {
                    retval += url.Substring(url.IndexOf(":"));
                }
                else
                {
                    retval += url.Substring(url.IndexOf("/"));
                }
            }

            

            if (stripEnd && retval.Length > 0 && retval.Count('/') > 3)
            {
                List<int> slashPositions = retval.Pos('/');
                retval = retval.Substring(0, slashPositions[2]);
            }
            else if (stripEnd && retval.Length > 0 && retval[retval.Length - 1] != '/' && retval.Count('/') == 3)
            {                
                retval = retval.Substring(0, retval.LastIndexOf('/'));
            }

            if (!retval.EndsWith("/"))
            {
                retval += "/";
            }

            return retval;
        }
    }
}
