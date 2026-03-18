using System;

namespace filesp.Monitors
{
    public class CpuMonitor : BaseMonitor
    {
        protected override double ReadMetric()
        {
            Console.WriteLine($"\n[Датчик] Чтение CPU: {MockValue}%");
            return MockValue;
        }

        protected override bool IsCritical(double value)
        {
            return value > 85.0; // Критично, если больше 85%
        }

        protected override EventData CreateEventData(double value)
        {
            return new EventData
            {
                MetricName = "CPU Load",
                Value = value,
                Message = "Критическая перегрузка процессора!"
            };
        }
    }
}