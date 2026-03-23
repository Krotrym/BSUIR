using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class Insurance
    {
        private string policyNumber;      
        private string insuranceType;     
        private double cost;             
        private bool isActive;            
        public string PolicyNumber
        {
            get => policyNumber; 
            set { policyNumber = value; }
        }

        public string InsuranceType
        {
            get => insuranceType; 
            set { insuranceType = value; }
        }

        public double Cost
        {
            get => cost;
            set { cost = value; }
        }

        public bool IsActive
        {
            get => isActive;
            set { isActive = value; }
        }

        public Insurance()
        {
            policyNumber = "";
            insuranceType = "";
            cost = 0;
            isActive = false;
        }
        public void Activate()
        {
            isActive = true;
            Console.WriteLine("Страховка " + policyNumber + " активирована");
        }
        public void Cancel()
        {
            isActive = false;
            Console.WriteLine("Страховка " + policyNumber + " отменена");
        }
        public void ShowInfo()
        {
            Console.WriteLine("Полис: " + policyNumber);
            Console.WriteLine("Тип: " + insuranceType);
            Console.WriteLine("Стоимость: " + cost);
            Console.WriteLine("Статус: " + (isActive ? "Активна" : "Неактивна"));
        }
    }

}
