using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratornay6.Entities
{
    public class Country
    {
        public string Name { get; set; }
        public string Name_ID { get; set; }
        public decimal Value { get; set; } = 0;
        public Country(string name, string name_ID)
        {
            Name = name;
            Name_ID = name_ID;
        }
    }
}
