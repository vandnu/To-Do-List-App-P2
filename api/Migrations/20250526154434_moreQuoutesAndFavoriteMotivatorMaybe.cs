using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class moreQuoutesAndFavoriteMotivatorMaybe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    favoriteMotivator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToDoTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Deadline = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoTasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Quotes",
                columns: new[] { "Id", "Author", "Text" },
                values: new object[,]
                {
                    { 1, "V", "Gør det i dag - det som du ellers kan overveje at udsætte til i morgen!" },
                    { 2, "A", "Små steps/sejre hver dag (mod et større mål) kan føre til store resultater." },
                    { 3, "Steve Jobs", "The only way to do great work is to love what you do." },
                    { 4, "Albert Einstein", "Strive not to be a success, but rather to be of value." },
                    { 5, "Steve Jobs", "Your time is limited, so don’t waste it living someone else’s life." },
                    { 6, "Buddha", "The mind is everything. What you think you become." },
                    { 7, "Peter Drucker", "The best way to predict the future is to create it." },
                    { 8, "Winston Churchill", "Success is not final, failure is not fatal: It is the courage to continue that counts." },
                    { 9, "Theodore Roosevelt", "Believe you can and you're halfway there." },
                    { 10, "William James", "Act as if what you do makes a difference. It does." },
                    { 11, "Confucius", "It does not matter how slowly you go as long as you do not stop." },
                    { 12, "Eleanor Roosevelt", "The future belongs to those who believe in the beauty of their dreams." },
                    { 13, "William Butler Yeats", "Do not wait to strike till the iron is hot; but make it hot by striking." },
                    { 14, "Walt Whitman", "Keep your face always toward the sunshine, and shadows will fall behind you." },
                    { 15, "Ralph Waldo Emerson", "What lies behind us and what lies before us are tiny matters compared to what lies within us." }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedTime", "Email", "PasswordHash", "Role", "Username", "favoriteMotivator" },
                values: new object[] { 1, new DateTime(2025, 4, 29, 12, 0, 0, 0, DateTimeKind.Utc), "test@ellernoget.com", "$2a$11$IsepszmC6snjPcfCcAmo5uctrPqXcLzRRoEVkpywIOw2kG7T./fOu", "Admin", "admin", null });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoTasks_UserId",
                table: "ToDoTasks",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropTable(
                name: "ToDoTasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
