using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Importer.WApp.Migrations.PokemonDb
{
    /// <inheritdoc />
    public partial class ImporterDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Films",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TimesWatched = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Films", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Imports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    ImportedIn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ErrorMessage = table.Column<string>(type: "TEXT", nullable: false),
                    FileContent = table.Column<byte[]>(type: "BLOB", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchPokemon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PokemonId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchPokemon", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Total = table.Column<int>(type: "INTEGER", nullable: false),
                    HP = table.Column<int>(type: "INTEGER", nullable: false),
                    Attack = table.Column<int>(type: "INTEGER", nullable: false),
                    Defense = table.Column<int>(type: "INTEGER", nullable: false),
                    SpeedAttack = table.Column<int>(type: "INTEGER", nullable: false),
                    SpeedDefense = table.Column<int>(type: "INTEGER", nullable: false),
                    Speed = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyPokemons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ImportedIn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PokemonId = table.Column<int>(type: "INTEGER", nullable: false),
                    PokemonId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyPokemons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyPokemons_Pokemons_PokemonId",
                        column: x => x.PokemonId,
                        principalTable: "Pokemons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MyPokemons_Pokemons_PokemonId1",
                        column: x => x.PokemonId1,
                        principalTable: "Pokemons",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "Id", "Attack", "Defense", "HP", "Name", "Speed", "SpeedAttack", "SpeedDefense", "Total", "Type" },
                values: new object[,]
                {
                    { 1, 49, 49, 45, "Bulbasaur", 45, 65, 65, 318, 0 },
                    { 2, 49, 49, 47, "Ivysaur", 45, 65, 65, 405, 1 },
                    { 3, 49, 49, 58, "Venusaur", 45, 65, 65, 525, 2 },
                    { 4, 49, 49, 33, "Charmander", 45, 65, 65, 625, 6 },
                    { 5, 49, 49, 87, "Charmeleon", 45, 65, 65, 309, 4 },
                    { 6, 49, 49, 26, "Charizard", 45, 65, 65, 578, 10 },
                    { 7, 49, 49, 45, "Squirtle", 45, 65, 65, 523, 7 },
                    { 8, 95, 49, 45, "Wartortle", 45, 65, 65, 643, 5 },
                    { 9, 49, 49, 15, "Blastoise", 45, 65, 65, 641, 8 },
                    { 10, 49, 49, 45, "Caterpie", 45, 65, 65, 312, 13 },
                    { 11, 96, 49, 45, "Metapod", 45, 65, 65, 541, 3 },
                    { 12, 49, 49, 45, "Butterfree", 45, 65, 65, 453, 11 },
                    { 13, 48, 49, 45, "Weedle", 45, 65, 65, 245, 9 },
                    { 14, 49, 49, 45, "Kakuna", 45, 65, 65, 513, 12 },
                    { 15, 49, 49, 45, "Beedrill", 45, 65, 65, 316, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyPokemons_PokemonId",
                table: "MyPokemons",
                column: "PokemonId");

            migrationBuilder.CreateIndex(
                name: "IX_MyPokemons_PokemonId1",
                table: "MyPokemons",
                column: "PokemonId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Films");

            migrationBuilder.DropTable(
                name: "Imports");

            migrationBuilder.DropTable(
                name: "MatchPokemon");

            migrationBuilder.DropTable(
                name: "MyPokemons");

            migrationBuilder.DropTable(
                name: "Pokemons");
        }
    }
}
