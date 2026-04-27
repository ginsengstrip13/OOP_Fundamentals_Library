using lab8.Core;

namespace lab8.Components;

/// <summary>
/// ConcreteColleague: простой UI-уровень.
/// Диспетчер принимает команды пользователя и передает их посреднику.
/// </summary>
public class Dispatcher : Colleague
{
    public void CommandAddDocument(Document document)
    {
        // UI-уровень не кладет документ в очередь напрямую.
        // Он отправляет пользовательскую команду посреднику.
        NotifyMediator(Events.AddDocument, document);
    }

    public void CommandProcessQueue()
    {
        // Запуск обработки очереди также проходит через Mediator.
        NotifyMediator(Events.ProcessQueue);
    }

    public void CommandRepairPrinter()
    {
        // Диспетчер не вызывает Printer.Repair() напрямую, чтобы сохранить слабую связанность.
        NotifyMediator(Events.RepairPrinter);
    }
}
