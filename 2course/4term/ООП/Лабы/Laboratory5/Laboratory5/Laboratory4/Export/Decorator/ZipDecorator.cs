using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace Laboratory4.Export.Decorator
{
    using System.IO.Compression;

    public class ZipSaver
    {
        private readonly IDataSaver _inner;

        public ZipSaver(IDataSaver inner)
        {
            _inner = inner;
        }

        public void SaveToZip(string data, string zipPath, string innerFileName)
        {
            using var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create);
            var entry = zip.CreateEntry(innerFileName);

            using var entryStream = entry.Open();
            _inner.Save(data, entryStream);
        }
    }




}
