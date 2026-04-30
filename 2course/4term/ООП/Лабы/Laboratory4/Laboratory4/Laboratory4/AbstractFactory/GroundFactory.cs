using Laboratory4.TypeTransport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.AbstractFactory
{
    internal class GroundFactory : IAbstractFactory
    {
        public Transport CreateTransport(double consumption, double speed, string type)
        {
            return new GroundTransport(consumption, speed, type);
        }
    }
}
