using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4
{
    public class Result
    {
        public string Type;
        public double Cost;
        public double Time;
        public Result() { }
        public Result(string type, double cost, double time)
        {
            Type = type;
            Cost = cost;
            Time = time;
        }
    }
}
