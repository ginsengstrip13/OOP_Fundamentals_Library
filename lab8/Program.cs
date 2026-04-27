using lab8.Components;
using lab8.Core;

namespace lab8;

internal static class Program
{
    private static void Main()
    {
        // Включаем UTF-8, чтобы русские сообщения корректно отображались в консоли.
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Создаем коллег паттерна Mediator.
        // На этом этапе они еще не знают друг о друге и не имеют прямых ссылок.
        var printer = new Printer();
        var queue = new PrintQueue();
        var logger = new Logger();
        var dispatcher = new Dispatcher();

        // Посредник получает ссылки на коллег и сам настраивает их связь с собой.
        // После этого все команды идут через mediator.Notify(...).
        var mediator = new PrintSystemMediator(printer, queue, logger, dispatcher);

        // Документы являются Context в паттерне State.
        // Каждый документ создается в состоянии New.
        var contract = new Document("Договор поставки");
        var invoice = new Document("Счет на оплату");
        var report = new Document("Годовой отчет");

        // Документы тоже участвуют в Mediator как коллеги, поэтому регистрируем их у посредника.
        mediator.RegisterDocument(contract);
        mediator.RegisterDocument(invoice);
        mediator.RegisterDocument(report);

        Console.WriteLine("=== Мини-система управления очередью печати ===");

        Console.WriteLine("\n--- Сценарий 1: успешная печать ---");
        // Диспетчер имитирует UI-уровень: пользователь добавляет документы и запускает очередь.
        dispatcher.CommandAddDocument(contract);
        dispatcher.CommandAddDocument(invoice);
        dispatcher.CommandProcessQueue();

        Console.WriteLine("\n--- Сценарий 2: ошибка принтера и восстановление ---");
        // Принтер специально "ломается" на выбранном документе.
        // Документ должен перейти из Printing в Error и получить право на повторную очередь.
        printer.BreakOnDocument("Годовой отчет");
        dispatcher.CommandAddDocument(report);
        dispatcher.CommandProcessQueue();

        // После ремонта документ в состоянии Error отправляется на печать повторно.
        dispatcher.CommandRepairPrinter();
        dispatcher.CommandAddDocument(report);
        dispatcher.CommandProcessQueue();

        Console.WriteLine("\n--- Сценарий 3: проверка финального состояния ---");
        // contract уже находится в Done, поэтому состояние само запрещает повторное добавление.
        dispatcher.CommandAddDocument(contract);
        dispatcher.CommandProcessQueue();

        Console.WriteLine("\n=== Работа системы завершена ===");
    }
}
