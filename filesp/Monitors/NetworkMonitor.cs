using System;

namespace filesp.Monitors
{
    public class NetworkMonitor : BaseMonitor
    {
        protected override double ReadMetric()
        {
            Console.WriteLine($"\n[Датчик] Чтение сети: {MockValue} МБ/с");
            return MockValue;
        }

        protected override bool IsCritical(double value)
        {
            return value > 100.0; // Допустим, больше 100 МБ/с — это аномалия
        }

        protected override EventData CreateEventData(double value)
        {
            return new EventData
            {
                MetricName = "Network Traffic",
                Value = value,
                Message = "Аномально высокая сетевая активность!"
            };
        }
    }
}