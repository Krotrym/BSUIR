using Laboratory4.TypeTransport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.AbstractFactory
{
    internal interface IAbstractFactory 
    {
        Transport CreateTransport(double consumption, double speed, string type);
    }
}
