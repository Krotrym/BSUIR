using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class Employee
    {
        private string fullName;
        private string position;
        public string FullName
        {
            get => fullName;

            set
            {
                fullName = value;
            }
        }
        public string Position
        {
            get => position;

            set
            {
                position = value;
            }
        }

        public Employee(string fullName, string position)
        {
            FullName = fullName;
            Position = position;
        }
    }

}
