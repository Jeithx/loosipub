using System.IO.Compression;

namespace CoreXNugetPackage.Utilities.Compressor
{
    public static class Compressor
    {
        public static byte[] Compress(byte[] data)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gzipStream.Write(data, 0, data.Length);
                    gzipStream.Close();
                    result = memoryStream.ToArray();
                }
            }
            return result;
        }

        public static byte[] Decompress(byte[] data)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(data, 0, data.Length);
                memoryStream.Position = 0L;
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress, true))
                {
                    using (MemoryStream memoryStream2 = new MemoryStream())
                    {
                        byte[] array = new byte[64];
                        for (int i = gzipStream.Read(array, 0, array.Length); i > 0; i = gzipStream.Read(array, 0, array.Length))
                        {
                            memoryStream2.Write(array, 0, i);
                        }
                        gzipStream.Close();
                        result = memoryStream2.ToArray();
                    }
                }
            }
            return result;
        }
    }
}
