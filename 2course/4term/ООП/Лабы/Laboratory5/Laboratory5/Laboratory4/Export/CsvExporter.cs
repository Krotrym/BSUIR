using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.Export
{
    public class CsvSaver : IDataSaver
    {
        public void Save(string data, Stream output)
        {
            using var writer = new StreamWriter(output, leaveOpen: true);
            writer.Write(data);
        }
    }

}
