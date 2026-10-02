using WebApiHome4.Models;

namespace WebApiHome4.Repositories;

public interface IBookRepository
{
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken);
    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Book>> SearchByAuthorAsync(string author, CancellationToken cancellationToken);
    Task<Book> AddAsync(Book book, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Book book, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
