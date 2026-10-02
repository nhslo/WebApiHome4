using Microsoft.EntityFrameworkCore;
using WebApiHome4.Data;
using WebApiHome4.Models;

namespace WebApiHome4.Repositories;

public class BookRepository(BookCatalogDbContext dbContext) : IBookRepository
{
    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken) =>
        await dbContext.Books.AsNoTracking().OrderBy(book => book.Id).ToListAsync(cancellationToken);

    public Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Books.AsNoTracking().FirstOrDefaultAsync(book => book.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Book>> SearchByAuthorAsync(string author, CancellationToken cancellationToken) =>
        await dbContext.Books.AsNoTracking()
            .Where(book => EF.Functions.Like(book.Author, $"%{author}%"))
            .OrderBy(book => book.Id)
            .ToListAsync(cancellationToken);

    public async Task<Book> AddAsync(Book book, CancellationToken cancellationToken)
    {
        dbContext.Books.Add(book);
        await dbContext.SaveChangesAsync(cancellationToken);
        return book;
    }

    public async Task<bool> UpdateAsync(Book book, CancellationToken cancellationToken)
    {
        if (!await dbContext.Books.AnyAsync(item => item.Id == book.Id, cancellationToken)) return false;
        dbContext.Books.Update(book);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var book = await dbContext.Books.FindAsync([id], cancellationToken);
        if (book is null) return false;
        dbContext.Books.Remove(book);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
