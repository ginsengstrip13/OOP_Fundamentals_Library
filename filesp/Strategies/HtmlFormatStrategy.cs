namespace filesp.Strategies
{
    public class HtmlFormatStrategy : IFormatStrategy
    {
        public string Format(EventData data)
        {
            return $"<div class='alert'><b>{data.MetricName}</b>: {data.Value} <i>({data.Message})</i></div>";
        }
    }
}