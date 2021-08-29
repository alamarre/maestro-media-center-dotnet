using System;
using System.IO;
using D2L.Hypermedia.Siren;

namespace MaestroServer.Hypermedia.Utilities
{
    public class CanonicalUrl
    {
        public Uri GetCanonicalUrl(string relativePath)
        {
            var builder = new UriBuilder(Environment.GetEnvironmentVariable("SERVER_NAME"));
            builder.Path = relativePath;
            return builder.Uri;
        }

        public SirenLink GetCanonicalLink(string relativePath, params string[] rel)
        {
            return new SirenLink(rel: rel, href: GetCanonicalUrl(relativePath));
        }


    }
}
