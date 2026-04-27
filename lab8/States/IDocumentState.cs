using lab8.Core;

namespace lab8.States;

/// <summary>
/// State: интерфейс состояния документа.
/// Каждое состояние само решает, какие действия допустимы и какие переходы выполнить.
/// </summary>
public interface IDocumentState
{
    /// <summary>
    /// Человекочитаемое имя состояния для вывода в лог.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Реакция состояния на попытку добавить документ в очередь.
    /// </summary>
    void AddToQueue(Document document);

    /// <summary>
    /// Реакция состояния на команду начать печать.
    /// </summary>
    void Print(Document document);

    /// <summary>
    /// Реакция состояния на успешное завершение печати.
    /// </summary>
    void CompletePrinting(Document document);

    /// <summary>
    /// Реакция состояния на ошибку печати.
    /// </summary>
    void FailPrinting(Document document);

    /// <summary>
    /// Реакция состояния на команду сброса/восстановления.
    /// </summary>
    void Reset(Document document);
}
