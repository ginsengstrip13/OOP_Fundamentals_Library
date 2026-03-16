using ComputerConfigurator.Models;

namespace ComputerConfigurator.Builders
{
    public class ComputerBuilder : IComputerBuilder
    {
        private Computer _computer = new Computer();

        public IComputerBuilder SetCPU(string cpu)
        {
            _computer.CPU = cpu;
            return this;
        }

        public IComputerBuilder SetRAM(int ram)
        {
            _computer.RAM = ram;
            return this;
        }

        public IComputerBuilder SetGPU(string gpu)
        {
            _computer.GPU = gpu;
            return this;
        }

        public IComputerBuilder AddAccessory(string accessory)
        {
            _computer.Accessories.Add(accessory);
            return this;
        }

        public Computer Build()
        {
            Computer result = _computer;
            _computer = new Computer(); // Сброс для следующей сборки
            return result;
        }
    }
}