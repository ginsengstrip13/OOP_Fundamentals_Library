using lab8.Components;

namespace lab8.Core;

/// <summary>
/// ConcreteMediator: центральный объект, который координирует документы,
/// принтер, очередь, логгер и диспетчер без прямых связей между компонентами.
/// </summary>
public class PrintSystemMediator : IMediator
{
    private readonly Printer _printer;
    private readonly PrintQueue _queue;
    private readonly Logger _logger;
    private readonly Dispatcher _dispatcher;

    public PrintSystemMediator(Printer printer, PrintQueue queue, Logger logger, Dispatcher dispatcher)
    {
        // Посредник хранит ссылки на всех коллег, потому что именно он организует их взаимодействие.
        _printer = printer;
        _queue = queue;
        _logger = logger;
        _dispatcher = dispatcher;

        // Коллеги получают ссылку только на IMediator.
        // Они не знают, какие еще объекты есть в системе.
        _printer.SetMediator(this);
        _queue.SetMediator(this);
        _logger.SetMediator(this);
        _dispatcher.SetMediator(this);
    }

    public void RegisterDocument(Document document)
    {
        // Документ тоже коллега: его состояния отправляют события посреднику через document.SendEvent(...).
        document.SetMediator(this);
        _logger.WriteMessage($"Документ '{document.Title}' создан. Состояние: {document.StateName}.");
    }

    public void Notify(Colleague sender, string eventName, Document? document = null)
    {
        // Центральный диспетчер событий Mediator.
        // Здесь находится вся координация: кто должен отреагировать на событие и в каком порядке.
        switch (eventName)
        {
            case Events.AddDocument:
                // Команда пришла от Dispatcher: пусть сам документ через State решит,
                // можно ли сейчас добавлять его в очередь.
                RequireDocument(document).AddToQueue();
                break;

            case Events.AddToQueue:
                // Состояние документа разрешило добавление, поэтому Mediator передает документ очереди.
                _queue.EnqueueItem(RequireDocument(document));
                break;

            case Events.Enqueued:
                // Очередь не пишет лог напрямую, она только сообщает событие Mediator.
                _logger.WriteMessage($"Документ '{RequireDocument(document).Title}' добавлен в очередь.");
                break;

            case Events.QueueDuplicate:
                _logger.WriteMessage($"Документ '{RequireDocument(document).Title}' уже находится в очереди.");
                break;

            case Events.ProcessQueue:
                // Команда обработки очереди пришла от Dispatcher.
                ProcessQueue();
                break;

            case Events.Dequeued:
                _logger.WriteMessage($"Документ '{RequireDocument(document).Title}' извлечен из очереди.");
                break;

            case Events.QueueEmpty:
                _logger.WriteMessage("Очередь печати пуста.");
                break;

            case Events.RequestPrint:
                // Документ перешел в Printing и запросил физическую печать.
                _printer.StartPrint(RequireDocument(document));
                break;

            case Events.PrintSuccess:
                // Принтер сообщил об успехе. Mediator переводит документ через State в Done.
                HandlePrintSuccess(RequireDocument(document));
                break;

            case Events.PrintFailed:
                // Принтер сообщил об ошибке. Mediator переводит документ через State в Error.
                HandlePrintFailure(RequireDocument(document));
                break;

            case Events.RepairPrinter:
                _printer.Repair();
                break;

            case Events.PrinterRepaired:
                _logger.WriteMessage("Принтер восстановлен после ошибки.");
                break;

            case Events.CannotAddPrinting:
                _logger.WriteMessage($"Документ '{RequireDocument(document).Title}' сейчас печатается и не может быть добавлен повторно.");
                break;

            case Events.CannotAddDone:
                _logger.WriteMessage($"Документ '{RequireDocument(document).Title}' уже напечатан. Финальное состояние запрещает повторную очередь.");
                break;

            case Events.CannotPrintDone:
                _logger.WriteMessage($"Документ '{RequireDocument(document).Title}' уже находится в состоянии Done.");
                break;

            case Events.CannotPrintPrinting:
                _logger.WriteMessage($"Документ '{RequireDocument(document).Title}' уже находится в состоянии Printing.");
                break;

            case Events.StateChanged:
                // Любая смена состояния фиксируется в логе как важное событие FSM.
                _logger.WriteMessage($"Документ '{RequireDocument(document).Title}' перешел в состояние {RequireDocument(document).StateName}.");
                break;

            default:
                _logger.WriteMessage($"Неизвестное событие: {eventName}.");
                break;
        }
    }

    private void ProcessQueue()
    {
        if (_queue.Count == 0)
        {
            // Через DequeueItem очередь сама сообщит событие QueueEmpty.
            _queue.DequeueItem();
            return;
        }

        while (_queue.Count > 0)
        {
            // FIFO: каждый раз забираем первый документ из очереди.
            var document = _queue.DequeueItem();

            // Дальше снова работает State: документ сам решает, можно ли начинать печать.
            document?.Print();
        }
    }

    private void HandlePrintSuccess(Document document)
    {
        // Успех печати не меняет состояние напрямую в Printer.
        // Переход Printing -> Done выполняется через текущее состояние документа.
        document.CompletePrinting();
        _logger.WriteMessage($"Документ '{document.Title}' успешно напечатан.");
    }

    private void HandlePrintFailure(Document document)
    {
        // Ошибка печати также обрабатывается через State.
        // Для Printing это переход в Error, после которого документ можно отправить повторно.
        document.FailPrinting();
        _logger.WriteMessage($"Ошибка печати документа '{document.Title}'. Документ можно отправить повторно.");
    }

    private static Document RequireDocument(Document? document)
    {
        // Защитная проверка: часть событий не несет Document, но события печати и очереди обязаны его иметь.
        return document ?? throw new ArgumentNullException(nameof(document), "Для события требуется документ.");
    }
}
