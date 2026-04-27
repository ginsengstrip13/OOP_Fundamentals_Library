namespace lab8.Core;

/// <summary>
/// Mediator: общий интерфейс посредника для обмена событиями между коллегами.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Единый канал событий: коллега-отправитель сообщает имя события и, при необходимости, документ.
    /// Конкретная реакция выбирается внутри реализации посредника.
    /// </summary>
    void Notify(Colleague sender, string eventName, Document? document = null);
}
