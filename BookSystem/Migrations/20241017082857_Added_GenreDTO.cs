using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSystem.Migrations
{
    /// <inheritdoc />
    public partial class Added_GenreDTO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Genres_Genresid",
                table: "BookGenre");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Genres",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Genresid",
                table: "BookGenre",
                newName: "GenresId");

            migrationBuilder.RenameIndex(
                name: "IX_BookGenre_Genresid",
                table: "BookGenre",
                newName: "IX_BookGenre_GenresId");

            migrationBuilder.AlterColumn<byte[]>(
                name: "CoverImage",
                table: "Books",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Genres_GenresId",
                table: "BookGenre",
                column: "GenresId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Genres_GenresId",
                table: "BookGenre");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Genres",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "GenresId",
                table: "BookGenre",
                newName: "Genresid");

            migrationBuilder.RenameIndex(
                name: "IX_BookGenre_GenresId",
                table: "BookGenre",
                newName: "IX_BookGenre_Genresid");

            migrationBuilder.AlterColumn<byte[]>(
                name: "CoverImage",
                table: "Books",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Genres_Genresid",
                table: "BookGenre",
                column: "Genresid",
                principalTable: "Genres",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
