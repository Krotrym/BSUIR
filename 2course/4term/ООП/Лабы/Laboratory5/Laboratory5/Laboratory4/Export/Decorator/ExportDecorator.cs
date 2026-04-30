using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.Export.Decorator
{
    public abstract class DataSaverDecorator : IDataSaver
    {
        protected readonly IDataSaver _inner;

        protected DataSaverDecorator(IDataSaver inner)
        {
            _inner = inner;
        }

        public abstract void Save(string data, Stream output);
    }


}
