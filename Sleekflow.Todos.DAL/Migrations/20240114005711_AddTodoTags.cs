using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sleekflow.Todos.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTodoTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTag_Todos_TodoId",
                table: "TodoTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoTag",
                table: "TodoTag");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TodoTag");

            migrationBuilder.RenameTable(
                name: "TodoTag",
                newName: "TodoTags");

            migrationBuilder.RenameIndex(
                name: "IX_TodoTag_TodoId",
                table: "TodoTags",
                newName: "IX_TodoTags_TodoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoTags",
                table: "TodoTags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTags_Todos_TodoId",
                table: "TodoTags",
                column: "TodoId",
                principalTable: "Todos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTags_Todos_TodoId",
                table: "TodoTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoTags",
                table: "TodoTags");

            migrationBuilder.RenameTable(
                name: "TodoTags",
                newName: "TodoTag");

            migrationBuilder.RenameIndex(
                name: "IX_TodoTags_TodoId",
                table: "TodoTag",
                newName: "IX_TodoTag_TodoId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TodoTag",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoTag",
                table: "TodoTag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTag_Todos_TodoId",
                table: "TodoTag",
                column: "TodoId",
                principalTable: "Todos",
                principalColumn: "Id");
        }
    }
}
