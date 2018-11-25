﻿namespace NetCoreTemplate.Web.Extensions
{
    using System.IO;

    public static class StreamExtensions
    {
        public static byte[] ReadAllBytes(this Stream instream)
        {
            if (instream is MemoryStream stream)
            {
                return stream.ToArray();
            }

            using (var memoryStream = new MemoryStream())
            {
                instream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
