namespace OOP_Fundamentals_Library
{
    public class ReportService
    {

        public static void GenerateReport(Person person)
        {
            Console.WriteLine("=== GENERATING REPORT ===");

            person.PrintInfo();
            Console.WriteLine("=========================\n");
        }
    }
}
