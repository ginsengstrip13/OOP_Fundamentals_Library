using lab8.Core;

namespace lab8.States;

/// <summary>
/// ConcreteState: документ создан и может быть помещен в очередь печати.
/// </summary>
public class NewState : IDocumentState
{
    public string Name => "New";

    public void AddToQueue(Document document)
    {
        // New -> очередь: сам Document не знает про PrintQueue.
        // Он только сообщает посреднику, что хочет быть добавленным.
        document.SendEvent(Events.AddToQueue);
    }

    public void Print(Document document)
    {
        // New -> Printing: документ извлечен из очереди и готов к физической печати.
        document.SetState(new PrintingState());

        // После смены состояния запрашиваем у Mediator запуск принтера.
        document.SendEvent(Events.RequestPrint);
    }

    public void CompletePrinting(Document document)
    {
        // Успешно завершить печать нового документа нельзя: печать еще не начиналась.
        Console.WriteLine($"[FSM: New] Документ '{document.Title}' еще не печатался.");
    }

    public void FailPrinting(Document document)
    {
        // Ошибка печати невозможна до начала печати.
        Console.WriteLine($"[FSM: New] Для документа '{document.Title}' не было процесса печати.");
    }

    public void Reset(Document document)
    {
        // Сброс не меняет состояние, потому что New уже является стартовым состоянием.
        Console.WriteLine($"[FSM: New] Документ '{document.Title}' уже готов к печати.");
    }
}
