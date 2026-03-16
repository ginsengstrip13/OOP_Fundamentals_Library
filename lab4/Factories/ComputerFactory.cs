using ComputerConfigurator.Models;
using ComputerConfigurator.Builders;

namespace ComputerConfigurator.Factories
{
    public class ComputerFactory
    {
        private readonly IComputerBuilder _builder;

        public ComputerFactory(IComputerBuilder builder)
        {
            _builder = builder;
        }

        public Computer CreateOfficePC()
        {
            return _builder.SetCPU("Intel Core i3").SetRAM(8).SetGPU("Integrated Intel UHD")
                           .AddAccessory("Офисная мышь").AddAccessory("Стандартная клавиатура").Build();
        }

        public Computer CreateGamingPC()
        {
            return _builder.SetCPU("AMD Ryzen 7 5800X").SetRAM(32).SetGPU("NVIDIA RTX 4080")
                           .AddAccessory("Мех. клавиатура").AddAccessory("Игровая мышь").Build();
        }

        public Computer CreateHomePC()
        {
            return _builder.SetCPU("Intel Core i5").SetRAM(16).SetGPU("NVIDIA RTX 3060")
                           .AddAccessory("Веб-камера").AddAccessory("Колонки").Build();
        }
    }
}