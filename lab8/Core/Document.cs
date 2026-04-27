using lab8.States;

namespace lab8.Core;

/// <summary>
/// Context паттерна State и одновременно коллега паттерна Mediator.
/// Класс не проверяет текущее состояние через if/switch, а делегирует поведение объекту состояния.
/// </summary>
public class Document : Colleague
{
    public Document(string title)
    {
        Title = title;

        // Начальное состояние конечного автомата: документ создан,
        // но еще не помещен в очередь и не печатался.
        State = new NewState();
    }

    /// <summary>
    /// Название документа используется в логах и при имитации ошибки принтера.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Текущее состояние документа. Именно оно определяет реакцию на команды.
    /// </summary>
    public IDocumentState State { get; private set; }

    public string StateName => State.Name;

    /// <summary>
    /// Единственный метод смены состояния.
    /// Важно: Document не решает, когда менять состояние; это делают ConcreteState-классы.
    /// </summary>
    public void SetState(IDocumentState state)
    {
        State = state;
        NotifyMediator(Events.StateChanged, this);
    }

    /// <summary>
    /// Команда добавления в очередь делегируется текущему состоянию.
    /// New и Error разрешают действие, Printing и Done запрещают.
    /// </summary>
    public void AddToQueue()
    {
        State.AddToQueue(this);
    }

    /// <summary>
    /// Команда печати также делегируется текущему состоянию.
    /// Это исключает проверки вида if (StateName == "...") внутри Document.
    /// </summary>
    public void Print()
    {
        State.Print(this);
    }

    /// <summary>
    /// Завершение печати обрабатывается состоянием.
    /// Для Printing это переход в Done.
    /// </summary>
    public void CompletePrinting()
    {
        State.CompletePrinting(this);
    }

    /// <summary>
    /// Ошибка печати обрабатывается состоянием.
    /// Для Printing это переход в Error.
    /// </summary>
    public void FailPrinting()
    {
        State.FailPrinting(this);
    }

    /// <summary>
    /// Сброс состояния нужен для демонстрации расширяемости автомата.
    /// В текущем сценарии повторная печать после ошибки идет напрямую из Error.
    /// </summary>
    public void Reset()
    {
        State.Reset(this);
    }

    /// <summary>
    /// Состояния не знают о конкретной очереди или принтере.
    /// Они только отправляют событие через документ как коллегу Mediator.
    /// </summary>
    public void SendEvent(string eventName)
    {
        NotifyMediator(eventName, this);
    }
}
