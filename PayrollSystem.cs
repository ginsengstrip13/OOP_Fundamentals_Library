using System;

namespace OOP_Fundamentals_Library
{
    public class PayrollSystem
    {

        public void ProcessSalary(Employee employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));

            Console.WriteLine($"Calculating salary for: {employee.Name}");
            Console.WriteLine($"Current Salary: {employee.Salary}");


            employee.ProcessPayroll();

            Console.WriteLine($"New Salary: {employee.Salary}");
            Console.WriteLine("-----------------------------");
        }
    }
}