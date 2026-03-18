using System.Collections.Generic;
using filesp.Subscribers;

namespace filesp.Monitors
{
    public abstract class BaseMonitor
    {
        // Список подписчиков на данный конкретный монитор
        private List<BaseSubscriber> _listeners = new List<BaseSubscriber>();

        // Для удобства симуляции в Program.cs будем задавать значение сюда
        public double MockValue { get; set; }

        // --- Управление подписчиками (Наблюдатель) ---
        public void Subscribe(BaseSubscriber subscriber)
        {
            _listeners.Add(subscriber);
        }

        public void Unsubscribe(BaseSubscriber subscriber)
        {
            _listeners.Remove(subscriber);
        }

        protected void Notify(EventData data)
        {
            foreach (var listener in _listeners)
            {
                listener.Update(data);
            }
        }

        // --- ШАБЛОННЫЙ МЕТОД проверки метрики ---
        public void RunCheck()
        {
            double currentValue = ReadMetric(); // Шаг 1: считать

            if (IsCritical(currentValue))       // Шаг 2: проверить порог
            {
                EventData data = CreateEventData(currentValue); // Шаг 3: сформировать данные
                Notify(data);                                   // Шаг 4: разослать
            }
        }

        // Абстрактные шаги, реализуемые в конкретных датчиках
        protected abstract double ReadMetric();
        protected abstract bool IsCritical(double value);
        protected abstract EventData CreateEventData(double value);
    }
}