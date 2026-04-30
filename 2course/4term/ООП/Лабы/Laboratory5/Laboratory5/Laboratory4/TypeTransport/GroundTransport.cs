using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.TypeTransport
{
    internal class GroundTransport : Transport
    {
        public GroundTransport() { }
        public GroundTransport(double consumption, double speed,string type)
        {
            ConsumptionPerkilometer = consumption;
            Speed = speed;
            Type = type;
        }
    }
}
