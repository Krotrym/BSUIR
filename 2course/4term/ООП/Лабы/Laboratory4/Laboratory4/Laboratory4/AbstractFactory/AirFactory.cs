using Laboratory4.TypeTransport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.AbstractFactory
{
    internal class AirFactory : IAbstractFactory
    {
        public Transport CreateTransport(double consumption, double speed, string type)
        {
            return new AirTransport(consumption, speed, type);
        }
    }
}
