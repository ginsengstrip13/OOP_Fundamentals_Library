namespace lab8.Core;

/// <summary>
/// Colleague: базовый класс компонентов системы.
/// Компонент знает только о посреднике и не хранит прямые ссылки на других коллег.
/// </summary>
public abstract class Colleague
{
    // protected, чтобы наследники могли отправлять события,
    // но внешние объекты не могли подменять посредника напрямую.
    protected IMediator? Mediator { get; private set; }

    /// <summary>
    /// Инъекция посредника. Ее выполняет PrintSystemMediator при настройке системы.
    /// </summary>
    public void SetMediator(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Единая точка отправки событий от коллеги к посреднику.
    /// Благодаря этому Printer, Logger, PrintQueue и Dispatcher не вызывают друг друга напрямую.
    /// </summary>
    protected void NotifyMediator(string eventName, Document? document = null)
    {
        if (Mediator is null)
        {
            throw new InvalidOperationException("Для коллеги не назначен посредник.");
        }

        Mediator.Notify(this, eventName, document);
    }
}
