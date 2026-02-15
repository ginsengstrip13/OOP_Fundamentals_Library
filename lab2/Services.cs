using System;

namespace SOLID_Fundamentals
{
    public interface IMessageSender
    {
        void Send(string to, string message);
    }

    public class EmailSender : IMessageSender
    {
        public void Send(string to, string message) => Console.WriteLine($"[Email] to {to}: {message}");
    }

    public class SmsSender : IMessageSender
    {
        public void Send(string to, string message) => Console.WriteLine($"[SMS] to {to}: {message}");
    }

    public class NotificationManager
    {
        private readonly IMessageSender _sender;

        public NotificationManager(IMessageSender sender)
        {
            _sender = sender;
        }

        public void Notify(string contact, string text)
        {
            _sender.Send(contact, text);
        }
    }
}