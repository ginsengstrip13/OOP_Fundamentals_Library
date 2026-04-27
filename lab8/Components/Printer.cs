using lab8.Core;

namespace lab8.Components;

/// <summary>
/// ConcreteColleague: принтер выполняет физическую печать.
/// Он не меняет состояние документа и не пишет в лог напрямую, а сообщает результат посреднику.
/// </summary>
public class Printer : Colleague
{
    // Название документа, на котором нужно один раз имитировать сбой.
    private string? _documentTitleToFail;

    // Флаг "поломки": пока он true, принтер не может успешно печатать.
    private bool _isBroken;

    // Флаг занятости показывает ограничение: принтер печатает только один документ за раз.
    private bool _isBusy;

    public void BreakOnDocument(string documentTitle)
    {
        // Метод используется в демонстрации, чтобы принтер сломался на конкретном документе.
        _documentTitleToFail = documentTitle;
    }

    public void StartPrint(Document document)
    {
        if (_isBusy)
        {
            Console.WriteLine("[Принтер] Принтер уже занят другим документом.");
            return;
        }

        _isBusy = true;
        Console.WriteLine($"[Принтер] Печать документа '{document.Title}'...");

        if (_isBroken || document.Title == _documentTitleToFail)
        {
            // Принтер не меняет состояние документа напрямую.
            // Он только сообщает посреднику о событии PrintFailed.
            _isBroken = true;
            _documentTitleToFail = null;
            _isBusy = false;
            NotifyMediator(Events.PrintFailed, document);
            return;
        }

        _isBusy = false;

        // Успешная печать также сообщается посреднику, а не документу или логгеру напрямую.
        NotifyMediator(Events.PrintSuccess, document);
    }

    public void Repair()
    {
        // Ремонт снимает флаг поломки и отправляет событие для логирования.
        _isBroken = false;
        NotifyMediator(Events.PrinterRepaired);
    }
}
