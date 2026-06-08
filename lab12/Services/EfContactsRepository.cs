using System.Collections.ObjectModel;
using lab12.Data;
using lab12.Models;
using Microsoft.EntityFrameworkCore;

namespace lab12.Services;

/// <summary>
/// Репозиторий загружает контакты из базы SQLite и сохраняет изменения через EF Core.
/// </summary>
public sealed class EfContactsRepository : IContactsRepository
{
    private readonly PhoneBookDbContext _context;

    public EfContactsRepository(PhoneBookDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Contacts = new ObservableCollection<Contact>(
            _context.Contacts
                .OrderBy(contact => contact.Name)
                .ToList());
    }

    public ObservableCollection<Contact> Contacts { get; }

    public void Add(Contact contact)
    {
        ArgumentNullException.ThrowIfNull(contact);

        _context.Contacts.Add(contact);
        _context.SaveChanges();
        Contacts.Add(contact);
    }

    public void Remove(Contact contact)
    {
        ArgumentNullException.ThrowIfNull(contact);

        _context.Contacts.Remove(contact);
        _context.SaveChanges();
        Contacts.Remove(contact);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
