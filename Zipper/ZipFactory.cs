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
        public byte[] CreateZip(IReadOnlyDictionary<string, byte[]> source)
        {
            using var zipmem = new System.IO.MemoryStream();
            using var zip = new System.IO.Compression.ZipArchive(zipmem, System.IO.Compression.ZipArchiveMode.Create, true);
            foreach (var buffer in source)
            {
                var entry = zip.CreateEntry(buffer.Key);
                using var zipMem = entry.Open();
                using var bufferMem=new System.IO.MemoryStream(buffer.Value);
                bufferMem.CopyTo(zipMem);
            }
            return zipmem.GetBuffer();
        }
    }
}
