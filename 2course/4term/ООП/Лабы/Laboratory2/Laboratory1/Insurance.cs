using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class Insurance
    {
        private string insuranceType;     
        private double cost;             
        private bool isActive;            

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

        public Insurance(string insuranceType, double cost, bool isActive)
        {
            InsuranceType = insuranceType;
            Cost = cost;
            IsActive = isActive;
        }
        public void Activate()
        {
            isActive = true;
            Console.WriteLine("Страховка активирована");
        }
        public void Cancel()
        {
            isActive = false;
            Console.WriteLine("Страховка отменена");
        }
        public void ShowInfo()
        {
            Console.WriteLine("Тип: " + insuranceType);
            Console.WriteLine("Стоимость: " + cost);
            Console.WriteLine("Статус: " + (isActive ? "Активна" : "Неактивна"));
        }
    }

}
