using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace Laboratory4.Export.Decorator
{
    using System.IO.Compression;

    public class ZipSaver : IDataSaver
    {
        private readonly IDataSaver _inner;

        public ZipSaver(IDataSaver inner)
        {
            _inner = inner;
        }

        public void Save(string data, Stream output)
        {
            using var zip = new ZipArchive(output, ZipArchiveMode.Create, leaveOpen: true);
            var entry = zip.CreateEntry("data.txt");

            using var entryStream = entry.Open();
            _inner.Save(data, entryStream);
        }
    }





}
