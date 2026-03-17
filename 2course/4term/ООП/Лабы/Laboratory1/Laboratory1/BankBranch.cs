using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class BankBranch
    {
        private string address;
        private string managerName;
        public string Address
        {
            get => address;

            set
            {
                address = value;
            }
        }
        public string ManagerName
        {
            get => managerName;

            set
            {
                managerName = value;
            }
        }

        public BankBranch(string address, string managerName)
        {
            Address = address;
            ManagerName = managerName;
        }
    }

}
