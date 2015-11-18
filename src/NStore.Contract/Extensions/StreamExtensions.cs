using System.IO;

namespace NStore.Contract.Extensions
{
    public static class StreamExtensions
    {
        // From: http://stackoverflow.com/a/221941/638087

        public static byte[] ToByteArray(this Stream input)
        {
            var buffer = new byte[16*1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}