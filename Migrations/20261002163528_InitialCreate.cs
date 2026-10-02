using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApiHome4.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    Author = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Description", "Price", "Title", "Year" },
                values: new object[,]
                {
                    { 1, "Джон Толкин", "Повесть о путешествии Бильбо Бэггинса.", 4500m, "Хоббит", 1937 },
                    { 2, "Джордж Оруэлл", "Антиутопический роман о тоталитарном обществе.", 3900m, "1984", 1949 },
                    { 3, "Джордж Оруэлл", "Аллегорическая повесть-притча.", 3200m, "Скотный двор", 1945 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
