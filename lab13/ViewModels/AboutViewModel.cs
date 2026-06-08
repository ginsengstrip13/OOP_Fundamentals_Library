namespace lab13.ViewModels;

public sealed class AboutViewModel
{
    public string AppName => "Телефонная книга";

    public string Version => "Лабораторная работа 13: CRUD через Entity Framework Core";

    public string Description =>
        "Приложение выполняет добавление, просмотр, редактирование и удаление контактов в SQLite-базе через ApplicationContext.";
}
