using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.TypeTransport
{
    internal class WaterTransport : Transport
    {
        public WaterTransport() { }

        public WaterTransport(double consumption, double speed, string type)
        {
            ConsumptionPerkilometer = consumption;
            Speed = speed;
            Type = type;
        }
    }
}
