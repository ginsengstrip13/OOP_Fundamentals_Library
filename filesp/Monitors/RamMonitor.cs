using System;

namespace filesp.Monitors
{
    public class RamMonitor : BaseMonitor
    {
        protected override double ReadMetric()
        {
            Console.WriteLine($"\n[Датчик] Чтение RAM: {MockValue} MB");
            return MockValue;
        }

        protected override bool IsCritical(double value)
        {
            return value > 8192; // Критично, если больше 8 ГБ
        }

        protected override EventData CreateEventData(double value)
        {
            return new EventData
            {
                MetricName = "RAM Usage",
                Value = value,
                Message = "Заканчивается оперативная память!"
            };
        }
    }
}