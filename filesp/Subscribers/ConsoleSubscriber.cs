using System;
using filesp.Strategies;

namespace filesp.Subscribers
{
    public class ConsoleSubscriber : BaseSubscriber
    {
        public ConsoleSubscriber(IFormatStrategy formatStrategy) : base(formatStrategy) { }

        protected override void DeliverMessage(string message)
        {
            Console.WriteLine($"[Console] => {message}");
        }
    }
}