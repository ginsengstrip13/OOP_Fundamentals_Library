using System;

namespace OOP_Fundamentals_Library
{
    // Тот самый базовый класс, которого не хватает в твоем списке файлов на скрине 2
    public abstract class Person
    {
        private string _name;
        private int _age;

        public string Name
        {
            get => _name;
            set => _name = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Name is empty");
        }

        public int Age
        {
            get => _age;
            set => _age = (value >= 0 && value <= 120) ? value : throw new ArgumentException("Invalid age");
        }

        // Конструктор, который принимает 2 аргумента (именно его ищет ошибка CS1729)
        protected Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public virtual void PrintInfo()
        {
            Console.WriteLine($"{this.GetType().Name}: {Name}, {Age} years old");
        }
    }
}