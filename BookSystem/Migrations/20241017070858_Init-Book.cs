using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookTitle = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookPages = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookTitle);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BookGenre",
                columns: table => new
                {
                    BooksBookTitle = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Genresid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenre", x => new { x.BooksBookTitle, x.Genresid });
                    table.ForeignKey(
                        name: "FK_BookGenre_Books_BooksBookTitle",
                        column: x => x.BooksBookTitle,
                        principalTable: "Books",
                        principalColumn: "BookTitle",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenre_Genre_Genresid",
                        column: x => x.Genresid,
                        principalTable: "Genre",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookGenre_Genresid",
                table: "BookGenre",
                column: "Genresid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookGenre");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Genre");
        }
    }
}
