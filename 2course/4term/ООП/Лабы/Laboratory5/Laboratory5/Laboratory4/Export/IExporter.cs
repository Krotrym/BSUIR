using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.Export
{
    public interface IDataSaver
    {
        void Save(string data, Stream output);
    }

    public interface IDataWriter
    {
        void Write(string data, Stream output);
    }


}
