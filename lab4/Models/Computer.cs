using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputerConfigurator.Models
{
    public class Computer
    {
        public string CPU { get; set; }
        public int RAM { get; set; }
        public string GPU { get; set; }
        public List<string> Accessories { get; set; } = new List<string>();

        // Поверхностное копирование
        public Computer ShallowClone()
        {
            return (Computer)this.MemberwiseClone();
        }

        // Глубокое копирование
        public Computer DeepClone()
        {
            Computer clone = (Computer)this.MemberwiseClone();
            clone.Accessories = new List<string>(this.Accessories);
            return clone;
        }

        public override string ToString()
        {
            string accessoriesStr = Accessories.Any() ? string.Join(", ", Accessories) : "Нет";
            return $"CPU: {CPU} | RAM: {RAM}GB | GPU: {GPU} | Допы: [{accessoriesStr}]";
        }
    }
}