using Laboratory4.TypeTransport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4
{
    internal class Delivery
    {
        public CargoBatch cargo;
        public Transport transport;
        public double distance;

        public Delivery(CargoBatch cargo, Transport transport, double distance)
        {
            this.cargo = cargo;
            this.transport = transport;
            this.distance = distance;
        }
        public double CalculateCost()
        {
            return cargo.GetTotalCargoCost() + distance * transport.ConsumptionPerkilometer;
        }

        public double CalculateTime()
        {
            return distance / transport.Speed;
        }
    }
}
