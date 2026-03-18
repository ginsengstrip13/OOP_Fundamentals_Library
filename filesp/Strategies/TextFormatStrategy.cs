namespace filesp.Strategies
{
    public class TextFormatStrategy : IFormatStrategy
    {
        public string Format(EventData data)
        {
            return $"[TEXT] {data.Timestamp:HH:mm:ss} | {data.MetricName}: {data.Value} -> {data.Message}";
        }
    }
}