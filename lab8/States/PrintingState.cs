using lab8.Core;

namespace lab8.States;

/// <summary>
/// ConcreteState: документ находится в процессе печати.
/// Повторное добавление в очередь в этом состоянии запрещено.
/// </summary>
public class PrintingState : IDocumentState
{
    public string Name => "Printing";

    public void AddToQueue(Document document)
    {
        // Документ уже печатается, поэтому повторное попадание в очередь запрещено.
        document.SendEvent(Events.CannotAddPrinting);
    }

    public void Print(Document document)
    {
        // Повторный старт печати не допускается: принтер уже обрабатывает документ.
        document.SendEvent(Events.CannotPrintPrinting);
    }

    public void CompletePrinting(Document document)
    {
        // Printing -> Done: успешное завершение печати переводит документ в финальное состояние.
        document.SetState(new DoneState());
    }

    public void FailPrinting(Document document)
    {
        // Printing -> Error: при сбое принтера документ можно будет отправить повторно.
        document.SetState(new ErrorState());
    }

    public void Reset(Document document)
    {
        // Технический сброс возвращает документ в стартовое состояние.
        document.SetState(new NewState());
    }
}
