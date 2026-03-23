using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class BankBranch
    {
        private string address;
        private string phone;
        private string workingHours;

        public string Address
        {
            get => address;
            set { address = value; }
        }

        public string Phone
        {
            get => phone;
            set { phone = value; }
        }

        public string WorkingHours
        {
            get => workingHours;
            set { workingHours = value; }
        }

        public BankBranch()
        {
            address = "";
            phone = "";
            workingHours = "09:00-18:00";
        }

        public string GetBranchInfo()
        {
            return "Отделение адрес: " + address + ", тел: " + phone;
        }
    }

}
