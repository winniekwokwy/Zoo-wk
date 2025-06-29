using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zoo.Migrations
{
    /// <inheritdoc />
    public partial class AddNewClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enclosures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    MaxCapicity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enclosures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransferRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AnimalId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastDateAtZoo = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    LocationOfTransfer = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferRecords_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZooKeepers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZooKeepers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZooKeeperEnclosures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ZooKeeperId = table.Column<int>(type: "INTEGER", nullable: false),
                    EnclosureId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZooKeeperEnclosures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZooKeeperEnclosures_Enclosures_EnclosureId",
                        column: x => x.EnclosureId,
                        principalTable: "Enclosures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZooKeeperEnclosures_ZooKeepers_ZooKeeperId",
                        column: x => x.ZooKeeperId,
                        principalTable: "ZooKeepers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animals_EnclosureId",
                table: "Animals",
                column: "EnclosureId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_SpeciesId",
                table: "Animals",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_AllSpecies_ClassificationId",
                table: "AllSpecies",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRecords_AnimalId",
                table: "TransferRecords",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_ZooKeeperEnclosures_EnclosureId",
                table: "ZooKeeperEnclosures",
                column: "EnclosureId");

            migrationBuilder.CreateIndex(
                name: "IX_ZooKeeperEnclosures_ZooKeeperId",
                table: "ZooKeeperEnclosures",
                column: "ZooKeeperId");

            migrationBuilder.AddForeignKey(
                name: "FK_AllSpecies_Classifications_ClassificationId",
                table: "AllSpecies",
                column: "ClassificationId",
                principalTable: "Classifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_AllSpecies_SpeciesId",
                table: "Animals",
                column: "SpeciesId",
                principalTable: "AllSpecies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Enclosures_EnclosureId",
                table: "Animals",
                column: "EnclosureId",
                principalTable: "Enclosures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllSpecies_Classifications_ClassificationId",
                table: "AllSpecies");

            migrationBuilder.DropForeignKey(
                name: "FK_Animals_AllSpecies_SpeciesId",
                table: "Animals");

            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Enclosures_EnclosureId",
                table: "Animals");

            migrationBuilder.DropTable(
                name: "TransferRecords");

            migrationBuilder.DropTable(
                name: "ZooKeeperEnclosures");

            migrationBuilder.DropTable(
                name: "Enclosures");

            migrationBuilder.DropTable(
                name: "ZooKeepers");

            migrationBuilder.DropIndex(
                name: "IX_Animals_EnclosureId",
                table: "Animals");

            migrationBuilder.DropIndex(
                name: "IX_Animals_SpeciesId",
                table: "Animals");

            migrationBuilder.DropIndex(
                name: "IX_AllSpecies_ClassificationId",
                table: "AllSpecies");
        }
    }
}
