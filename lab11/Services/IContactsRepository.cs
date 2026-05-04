using System.Collections.ObjectModel;
using lab11.Models;

namespace lab11.Services;

/// <summary>
/// Хранилище контактов отделяет данные от времени жизни экранных ViewModel.
/// </summary>
public interface IContactsRepository
{
    ObservableCollection<Contact> Contacts { get; }
}
