using System.Collections.ObjectModel;
using lab11.Models;

namespace lab11.Services;

/// <summary>
/// Простое хранилище в памяти для учебного приложения.
/// </summary>
public sealed class InMemoryContactsRepository : IContactsRepository
{
    // ObservableCollection уведомляет DataGrid об изменениях состава коллекции:
    // добавление и удаление строк сразу отражаются на экране.
    public ObservableCollection<Contact> Contacts { get; } = new();
}
