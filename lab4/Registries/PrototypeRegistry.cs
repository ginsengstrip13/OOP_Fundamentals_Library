using System;
using System.Collections.Generic;
using ComputerConfigurator.Models;

namespace ComputerConfigurator.Registries
{
    public class PrototypeRegistry
    {
        private static PrototypeRegistry _instance;
        private static readonly object _lock = new object();

        private Dictionary<string, Computer> _prototypes = new Dictionary<string, Computer>();

        private PrototypeRegistry() { }

        public static PrototypeRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PrototypeRegistry();
                        }
                    }
                }
                return _instance;
            }
        }

        public void AddPrototype(string key, Computer prototype)
        {
            _prototypes[key] = prototype;
        }

        public Computer GetPrototype(string key)
        {
            if (_prototypes.ContainsKey(key))
            {
                return _prototypes[key];
            }
            throw new ArgumentException($"Прототип {key} не найден.");
        }
    }
}