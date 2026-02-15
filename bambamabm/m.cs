using OOP_Fundamentals_Library;

internal class Mainproject
{
    
    static void Main(string[] args)
    {
        var customer = new Customer("John", 30);

        var employee = new Employee("Alice", 25, 50000, "Developer");

        var manager = new Manager("Bob", 40, 80000, "IT");

        manager.AddTeamMember(employee);

        employee.IncreaseSalary(55000);

        customer.PrintInfo();
        employee.PrintInfo();
        manager.PrintInfo();

        var payroll = new PayrollSystem();
        payroll.ProcessSalary(employee);
        payroll.ProcessSalary(manager);

        ReportService.GenerateReport(employee);
        ReportService.GenerateReport(manager);
    }
}