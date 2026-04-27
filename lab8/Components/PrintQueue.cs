using lab8.Core;

namespace lab8.Components;

/// <summary>
/// ConcreteColleague: очередь печати FIFO.
/// Очередь не запускает принтер напрямую, а только сообщает посреднику о событиях.
/// </summary>
public class PrintQueue : Colleague
{
    // Встроенная Queue<T> обеспечивает порядок FIFO: первый добавленный документ печатается первым.
    private readonly Queue<Document> _documents = new();

    public int Count => _documents.Count;

    public void EnqueueItem(Document document)
    {
        if (_documents.Contains(document))
        {
            // Очередь не пишет в лог сама, а сообщает посреднику о дубликате.
            NotifyMediator(Events.QueueDuplicate, document);
            return;
        }

        _documents.Enqueue(document);

        // После добавления очередь сообщает Mediator, чтобы тот записал событие в лог.
        NotifyMediator(Events.Enqueued, document);
    }

    public Document? DequeueItem()
    {
        if (_documents.Count == 0)
        {
            // Пустая очередь тоже событие системы, поэтому оно проходит через посредника.
            NotifyMediator(Events.QueueEmpty);
            return null;
        }

        var document = _documents.Dequeue();

        // Очередь не запускает печать сама: она только возвращает документ и сообщает событие.
        NotifyMediator(Events.Dequeued, document);
        return document;
    }
}
