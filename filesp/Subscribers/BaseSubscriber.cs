using filesp.Strategies;

namespace filesp.Subscribers
{
    public abstract class BaseSubscriber
    {
        protected IFormatStrategy _formatStrategy;

        public BaseSubscriber(IFormatStrategy formatStrategy)
        {
            _formatStrategy = formatStrategy;
        }

        // Метод, который вызывает Издатель
        public void Update(EventData data)
        {
            SendNotification(data);
        }

        // ШАБЛОННЫЙ МЕТОД: строгий порядок обработки и отправки сообщения
        public void SendNotification(EventData data)
        {
            string message = _formatStrategy.Format(data);

            PrepareChannel();
            DeliverMessage(message);
            CloseChannel();
        }

        // Хуки (необязательные шаги)
        protected virtual void PrepareChannel() { }
        protected virtual void CloseChannel() { }

        // Обязательный шаг для наследников
        protected abstract void DeliverMessage(string message);
    }
}