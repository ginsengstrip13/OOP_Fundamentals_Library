using lab8.Core;

namespace lab8.States;

/// <summary>
/// ConcreteState: печать завершилась ошибкой.
/// Документ разрешено повторно отправить в очередь.
/// </summary>
public class ErrorState : IDocumentState
{
    public string Name => "Error";

    public void AddToQueue(Document document)
    {
        // Error разрешает повторную отправку документа в очередь после восстановления принтера.
        document.SendEvent(Events.AddToQueue);
    }

    public void Print(Document document)
    {
        // Error -> Printing: повторная попытка печати после сбоя.
        document.SetState(new PrintingState());
        document.SendEvent(Events.RequestPrint);
    }

    public void CompletePrinting(Document document)
    {
        // Пока документ в Error, сигнал успешного завершения не должен приниматься.
        Console.WriteLine($"[FSM: Error] Ошибка документа '{document.Title}' не устранена.");
    }

    public void FailPrinting(Document document)
    {
        // Повторная ошибка не меняет состояние.
        Console.WriteLine($"[FSM: Error] Документ '{document.Title}' уже находится в состоянии ошибки.");
    }

    public void Reset(Document document)
    {
        // Error -> New: альтернативный способ вернуть документ к стартовому состоянию.
        document.SetState(new NewState());
    }
}
