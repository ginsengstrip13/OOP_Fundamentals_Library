using System;
using ComputerConfigurator.Models;
using ComputerConfigurator.Builders;
using ComputerConfigurator.Factories;
using ComputerConfigurator.Registries;

namespace ComputerConfigurator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Инициализация системы ===");

            IComputerBuilder builder = new ComputerBuilder();
            ComputerFactory factory = new ComputerFactory(builder);
            PrototypeRegistry registry = PrototypeRegistry.Instance;

            // 1. Создаем и сохраняем прототипы
            registry.AddPrototype("Office", factory.CreateOfficePC());
            registry.AddPrototype("Gaming", factory.CreateGamingPC());

            Console.WriteLine("Прототипы добавлены в Singleton реестр.\n");

            // 2. Тест Shallow Copy (Поверхностное копирование)
            Console.WriteLine("=== Тест: Shallow Copy ===");
            Computer originalOffice = registry.GetPrototype("Office");
            Computer shallowClone = originalOffice.ShallowClone();

            shallowClone.Accessories.Add("Бейдж сотрудника"); // Меняем список у клона

            Console.WriteLine("Оригинал: " + originalOffice);
            Console.WriteLine("Клон:     " + shallowClone);
            Console.WriteLine("Итог: Список изменился у обоих (общая ссылка в памяти).\n");

            // 3. Тест Deep Copy (Глубокое копирование)
            Console.WriteLine("=== Тест: Deep Copy ===");
            Computer originalGaming = registry.GetPrototype("Gaming");
            Computer deepClone = originalGaming.DeepClone();

            deepClone.Accessories.Add("VR Шлем"); // Меняем список у клона

            Console.WriteLine("Оригинал: " + originalGaming);
            Console.WriteLine("Клон:     " + deepClone);
            Console.WriteLine("Итог: Список клона независим, оригинал не пострадал.\n");

            Console.ReadLine();
        }
    }
}