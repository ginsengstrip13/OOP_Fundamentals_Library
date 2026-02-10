using System.Collections.Generic;

namespace OOP_Fundamentals_Library
{
    public class Manager : Employee
    {
        public string Department { get; private set; }


        private List<Employee> _team = new();
        public IReadOnlyList<Employee> Team => _team.AsReadOnly();

        public Manager(string name, int age, decimal salary, string department)
            : base(name, age, salary, "Manager")
        {
            Department = department;
        }

        public void AddTeamMember(Employee emp)
        {
            if (emp == null) throw new ArgumentNullException(nameof(emp));
            _team.Add(emp);
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"  Department: {Department}, Team size: {_team.Count}");
        }

        public void AssignTask(Employee emp, string task)
        {
            if (!_team.Contains(emp))
            {
                Console.WriteLine($"Error: {emp.Name} is not in {Name}'s team.");
                return;
            }
            Console.WriteLine($"Manager {Name} assigned task '{task}' to {emp.Name}");
        }


        public override void ProcessPayroll()
        {
            Console.WriteLine($"Processing executive payroll for Manager {Name}...");
            IncreaseSalary(2000);
        }

        public override decimal CalculateBonus(int yearsOfService, bool hasCertification)
        {
            decimal bonus = Salary * 0.2m;


            if (yearsOfService > 5) bonus += 500;
            if (hasCertification) bonus += 300;

            return bonus;
        }
    }
}