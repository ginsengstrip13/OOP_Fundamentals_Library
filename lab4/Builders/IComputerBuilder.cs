using ComputerConfigurator.Models;

namespace ComputerConfigurator.Builders
{
    public interface IComputerBuilder
    {
        IComputerBuilder SetCPU(string cpu);
        IComputerBuilder SetRAM(int ram);
        IComputerBuilder SetGPU(string gpu);
        IComputerBuilder AddAccessory(string accessory);
        Computer Build();
    }
}