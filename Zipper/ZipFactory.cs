using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zipper
{
    public class ZipFactory
    {
        public byte[] CreateZip(string filename, IReadOnlyDictionary<string, byte[]> source)
        {
            using var zipmem = new System.IO.MemoryStream();
            using var zip = new System.IO.Compression.ZipArchive(zipmem, System.IO.Compression.ZipArchiveMode.Create, true);
            var entry1 = zip.CreateEntry($"unhash_{filename}");
            foreach (var buffer in source)
            {
                var entry = zip.CreateEntry(buffer.Key);
                using var zipMem = entry1.Open();
                using var bufferMem=new System.IO.MemoryStream(buffer.Value);
                bufferMem.CopyTo(zipmem);
            }
            return zipmem.GetBuffer();
        }
    }
}
