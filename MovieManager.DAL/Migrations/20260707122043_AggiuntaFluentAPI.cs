using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaFluentAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Reviews",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Movies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Genres",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Directors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BrithDate",
                table: "Directors",
                newName: "BirthDate");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Actors",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reviews",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Movies",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Genres",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Directors",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "Directors",
                newName: "BrithDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Actors",
                newName: "id");
        }
    }
}
