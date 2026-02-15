using System.Xml.Linq;
using System;

namespace OOP_Fundamentals_Library
{
    public class Employee : Person
    {
        public decimal _salary;

        public string Position { get; private set; } // Сеттер приватный, менять только через методы

        public decimal Salary
        {
            get => _salary;
            protected set // Разрешаем менять зарплату только внутри класса и наследников
            {
                if (value < 0) throw new ArgumentException("Salary cannot be negative.");
                _salary = value;
            }
        }

        public Employee(string name, int age, decimal salary, string position)
            : base(name, age)
        {
            Salary = salary;
            Position = position;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"  Position: {Position}, Salary: {Salary:C}");
        }

        public void IncreaseSalary(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Increase amount must be positive.");
            Salary += amount;
        }

        // Полиморфизм: Виртуальный метод для обработки зарплаты (вместо if в PayrollSystem)
        public virtual void ProcessPayroll()
        {
            Console.WriteLine($"Processing base payroll for {Name}...");
            IncreaseSalary(1000); // Базовая надбавка
        }

        // Полиморфизм: Расчет бонуса перенесен внутрь класса
        public virtual decimal CalculateBonus(int yearsOfService, bool hasCertification)
        {
            decimal bonus = Salary * 0.1m; // 10% для обычного сотрудника

            if (yearsOfService > 5) bonus += 500;
            if (hasCertification) bonus += 300;

            return bonus;
        }
    }
}