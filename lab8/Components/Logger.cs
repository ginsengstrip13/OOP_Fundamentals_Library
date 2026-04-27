using lab8.Core;

namespace lab8.Components;

/// <summary>
/// ConcreteColleague: логгер фиксирует ключевые события системы.
/// Он вызывается посредником и не обращается к другим компонентам напрямую.
/// </summary>
public class Logger : Colleague
{
    public void WriteMessage(string message)
    {
        // Логгер является отдельным коллегой Mediator.
        // Он не решает, какие события произошли, а только фиксирует сообщения посредника.
        Console.WriteLine($"[Лог] {DateTime.Now:HH:mm:ss} | {message}");
    }
}
