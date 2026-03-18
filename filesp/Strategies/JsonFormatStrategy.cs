namespace filesp.Strategies
{
    public class JsonFormatStrategy : IFormatStrategy
    {
        public string Format(EventData data)
        {
            return $@"{{ ""time"": ""{data.Timestamp:HH:mm:ss}"", ""metric"": ""{data.MetricName}"", ""value"": {data.Value}, ""msg"": ""{data.Message}"" }}";
        }
    }
}