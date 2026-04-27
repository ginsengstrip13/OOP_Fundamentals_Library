namespace lab8.Core;

/// <summary>
/// Имена событий, которыми коллеги обмениваются только через посредника.
/// Константы позволяют не раскидывать строковые литералы по коду и проще видеть протокол Mediator.
/// </summary>
public static class Events
{
    // Команды от диспетчера и документа.
    public const string AddDocument = "AddDocument";
    public const string AddToQueue = "AddToQueue";
    public const string ProcessQueue = "ProcessQueue";
    public const string RequestPrint = "RequestPrint";

    // События результата печати.
    public const string PrintSuccess = "PrintSuccess";
    public const string PrintFailed = "PrintFailed";

    // События обслуживания принтера.
    public const string RepairPrinter = "RepairPrinter";
    public const string PrinterRepaired = "PrinterRepaired";

    // События очереди FIFO.
    public const string Enqueued = "Enqueued";
    public const string Dequeued = "Dequeued";
    public const string QueueEmpty = "QueueEmpty";
    public const string QueueDuplicate = "QueueDuplicate";

    // События, показывающие запреты конечного автомата состояний.
    public const string CannotAddPrinting = "CannotAddPrinting";
    public const string CannotAddDone = "CannotAddDone";
    public const string CannotPrintDone = "CannotPrintDone";
    public const string CannotPrintPrinting = "CannotPrintPrinting";

    // Общее событие смены состояния документа.
    public const string StateChanged = "StateChanged";
}
