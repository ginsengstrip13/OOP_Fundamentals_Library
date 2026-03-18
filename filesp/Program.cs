using System;
using filesp.Monitors;
using filesp.Strategies;
using filesp.Subscribers;

namespace filesp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ЗАПУСК СИСТЕМЫ МОНИТОРИНГА ===\n");

            
            var cpuMonitor = new CpuMonitor();
            var ramMonitor = new RamMonitor();
            var networkMonitor = new NetworkMonitor();

            var adminConsole = new ConsoleSubscriber(new TextFormatStrategy());
            var jsonFileLogger = new FileSubscriber(new JsonFormatStrategy(), "system_log.json");
            var webDashboard = new ConsoleSubscriber(new HtmlFormatStrategy());

            
            cpuMonitor.Subscribe(adminConsole);

            
            cpuMonitor.Subscribe(jsonFileLogger);
            ramMonitor.Subscribe(jsonFileLogger);

            
            ramMonitor.Subscribe(webDashboard);

            
            networkMonitor.MockValue = 150.5;
            networkMonitor.RunCheck(); 
            cpuMonitor.MockValue = 45.0;
            cpuMonitor.RunCheck(); 

            ramMonitor.MockValue = 4000;
            ramMonitor.RunCheck(); 

            
            cpuMonitor.MockValue = 92.5;
            cpuMonitor.RunCheck(); 

            ramMonitor.MockValue = 8500;
            ramMonitor.RunCheck(); 
            Console.WriteLine("\n=== РАБОТА СИСТЕМЫ ЗАВЕРШЕНА ===");
            Console.ReadLine();
        }
    }
}