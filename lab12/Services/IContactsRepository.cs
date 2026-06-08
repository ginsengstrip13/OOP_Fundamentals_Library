using System.Collections.ObjectModel;
using lab12.Models;

namespace lab12.Services;

/// <summary>
/// Хранилище контактов отделяет данные от времени жизни экранных ViewModel.
/// </summary>
public interface IContactsRepository
{
    ObservableCollection<Contact> Contacts { get; }

    void Add(Contact contact);

    void Remove(Contact contact);

    void SaveChanges();
}
