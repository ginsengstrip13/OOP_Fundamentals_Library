using System;

namespace filesp
{
    public class EventData
    {
        // Добавили = string.Empty; чтобы убрать предупреждения
        public string MetricName { get; set; } = string.Empty;
        public double Value { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}