using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.TypeTransport
{
    internal class AirTransport : Transport
    {
        public AirTransport() { }
        public AirTransport(double consumption, double speed, string type) 
        {
            ConsumptionPerkilometer = consumption;
            Speed = speed;
            Type = type;
        }
    }
}
