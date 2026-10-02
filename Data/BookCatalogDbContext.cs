using Microsoft.EntityFrameworkCore;
using WebApiHome4.Models;

namespace WebApiHome4.Data;

public class BookCatalogDbContext(DbContextOptions<BookCatalogDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(book => book.Title).HasMaxLength(160).IsRequired();
            entity.Property(book => book.Author).HasMaxLength(120).IsRequired();
            entity.Property(book => book.Description).HasMaxLength(2000);
            entity.Property(book => book.Price).HasPrecision(10, 2);
            entity.HasData(
                new Book { Id = 1, Title = "Хоббит", Author = "Джон Толкин", Year = 1937, Price = 4500m, Description = "Повесть о путешествии Бильбо Бэггинса." },
                new Book { Id = 2, Title = "1984", Author = "Джордж Оруэлл", Year = 1949, Price = 3900m, Description = "Антиутопический роман о тоталитарном обществе." },
                new Book { Id = 3, Title = "Скотный двор", Author = "Джордж Оруэлл", Year = 1945, Price = 3200m, Description = "Аллегорическая повесть-притча." });
        });
    }
}
