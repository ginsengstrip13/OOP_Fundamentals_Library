using lab8.Core;

namespace lab8.States;

/// <summary>
/// ConcreteState: финальное состояние успешно напечатанного документа.
/// Повторная печать и добавление в очередь запрещены.
/// </summary>
public class DoneState : IDocumentState
{
    public string Name => "Done";

    public void AddToQueue(Document document)
    {
        // Done - финальное состояние, повторная очередь нарушила бы модель автомата.
        document.SendEvent(Events.CannotAddDone);
    }

    public void Print(Document document)
    {
        // Уже напечатанный документ не должен печататься повторно.
        document.SendEvent(Events.CannotPrintDone);
    }

    public void CompletePrinting(Document document)
    {
        // Повторный сигнал успеха не меняет финальное состояние.
        Console.WriteLine($"[FSM: Done] Документ '{document.Title}' уже напечатан.");
    }

    public void FailPrinting(Document document)
    {
        // Из финального состояния нельзя перейти в Error.
        Console.WriteLine($"[FSM: Done] Документ '{document.Title}' уже находится в финальном состоянии.");
    }

    public void Reset(Document document)
    {
        // Финальное состояние специально не сбрасывается, чтобы сохранить строгую FSM.
        Console.WriteLine($"[FSM: Done] Финальное состояние нельзя сбросить.");
    }
}
