using System;

namespace SOLID_Fundamentals
{
    public interface IOrderPlacement
    {
        void CreateOrder(Order order);
    }

    public interface IOrderManagement
    {
        void DeleteOrder(int id);
        void UpdateOrder(Order order);
    }

    public interface IReportGenerator
    {
        void GenerateReport();
    }

    public class CustomerOperations : IOrderPlacement
    {
        public void CreateOrder(Order order) => Console.WriteLine("Клиент создал заказ.");
    }

    public class AdminOperations : IOrderPlacement, IOrderManagement, IReportGenerator
    {
        public void CreateOrder(Order order) => Console.WriteLine("Админ создал заказ.");
        public void DeleteOrder(int id) => Console.WriteLine($"Админ удалил заказ {id}.");
        public void UpdateOrder(Order order) => Console.WriteLine("Админ обновил заказ.");
        public void GenerateReport() => Console.WriteLine("Админ скачал отчет.");
    }
}