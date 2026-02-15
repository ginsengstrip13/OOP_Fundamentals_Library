using System;
using System.Collections.Generic;
using System.Linq;

namespace SOLID_Fundamentals
{
    public interface IPaymentService { void ProcessPayment(string method, decimal amount); }
    public interface IInventoryService { void UpdateInventory(List<string> items); }
    public interface INotificationService { void SendNotification(string to, string message); }
    public interface ILoggerService { void Log(string message); }

    public class PaymentService : IPaymentService
    {
        public void ProcessPayment(string method, decimal amount) => Console.WriteLine($"Оплата {amount} через {method} прошла.");
    }
    public class InventoryService : IInventoryService
    {
        public void UpdateInventory(List<string> items) => Console.WriteLine("Склад обновлен.");
    }
    public class EmailNotificationService : INotificationService
    {
        public void SendNotification(string to, string message) => Console.WriteLine($"Email для {to}: {message}");
    }
    public class ConsoleLogger : ILoggerService
    {
        public void Log(string message) => Console.WriteLine($"LOG: {message}");
    }

    public class OrderProcessor
    {
        private readonly IPaymentService _payment;
        private readonly IInventoryService _inventory;
        private readonly INotificationService _notification;
        private readonly ILoggerService _logger;

        public OrderProcessor(IPaymentService payment, IInventoryService inventory, INotificationService notification, ILoggerService logger)
        {
            _payment = payment;
            _inventory = inventory;
            _notification = notification;
            _logger = logger;
        }

        public void Process(Order order)
        {
            Console.WriteLine($"--- Обработка заказа {order.Id} ---");
            _payment.ProcessPayment(order.PaymentMethod, order.TotalAmount);
            _inventory.UpdateInventory(order.Items);
            _notification.SendNotification(order.CustomerEmail, "Заказ успешно обработан");
            _logger.Log($"Заказ {order.Id} завершен в {DateTime.Now}");
        }
    }
}