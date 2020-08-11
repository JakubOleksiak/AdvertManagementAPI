using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvertApiS18728.Migrations
{
    public partial class AddedTableStructureAndData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Building",
                columns: table => new
                {
                    IdBuilding = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(maxLength: 100, nullable: false),
                    StreetNumber = table.Column<int>(nullable: false),
                    City = table.Column<string>(maxLength: 100, nullable: false),
                    Height = table.Column<decimal>(type: "decimal(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Building_PK", x => x.IdBuilding);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    IdClient = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    Phone = table.Column<string>(maxLength: 100, nullable: false),
                    Login = table.Column<string>(maxLength: 100, nullable: false),
                    RefreshToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Client_PK", x => x.IdClient);
                });

            migrationBuilder.CreateTable(
                name: "Campaign",
                columns: table => new
                {
                    IdCampaign = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClient = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    PricePerSquareMeter = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    FromIdBuilding = table.Column<int>(nullable: false),
                    ToIdBuilding = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Campaign_PK", x => x.IdCampaign);
                    table.ForeignKey(
                        name: "Campaign_FromBuilding",
                        column: x => x.FromIdBuilding,
                        principalTable: "Building",
                        principalColumn: "IdBuilding",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Campaign_Client",
                        column: x => x.IdClient,
                        principalTable: "Client",
                        principalColumn: "IdClient",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Campaign_ToBuilding",
                        column: x => x.ToIdBuilding,
                        principalTable: "Building",
                        principalColumn: "IdBuilding",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Password",
                columns: table => new
                {
                    IdPassword = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClient = table.Column<int>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Password_PK", x => x.IdPassword);
                    table.ForeignKey(
                        name: "Client_Password",
                        column: x => x.IdClient,
                        principalTable: "Client",
                        principalColumn: "IdClient",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Salt",
                columns: table => new
                {
                    IdSalt = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClient = table.Column<int>(nullable: false),
                    SaltHash = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Salt_PK", x => x.IdSalt);
                    table.ForeignKey(
                        name: "Client_Salt",
                        column: x => x.IdClient,
                        principalTable: "Client",
                        principalColumn: "IdClient",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Banner",
                columns: table => new
                {
                    IdAdvertisement = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    IdCampaign = table.Column<int>(nullable: false),
                    Area = table.Column<decimal>(type: "decimal(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Banner_PK", x => x.IdAdvertisement);
                    table.ForeignKey(
                        name: "Banner_Campaign",
                        column: x => x.IdCampaign,
                        principalTable: "Campaign",
                        principalColumn: "IdCampaign",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Building",
                columns: new[] { "IdBuilding", "City", "Height", "Street", "StreetNumber" },
                values: new object[,]
                {
                    { 1, "Warszawa", 20.5m, "SuperStreet", 5 },
                    { 2, "Warszawa", 13.23m, "SuperStreet", 6 },
                    { 3, "Szczecin", 14.5m, "ExtraStreet", 2 },
                    { 4, "Szczecin", 22.3m, "ExtraStreet", 3 },
                    { 5, "Szczecin", 23.1m, "ExtraStreet", 4 }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "IdClient", "Email", "FirstName", "LastName", "Login", "Phone", "RefreshToken" },
                values: new object[,]
                {
                    { 1, "jkowalski@gmail.com", "Jan", "Kowalski", "Kowal", "666777888", null },
                    { 2, "wdzban@gmail.com", "Waclaw", "Dzban", "Dzbanuszek", "777888999", null },
                    { 3, "balgab@gmail.com", "Baltazar", "Gabka", "Balgab", "111222333", null },
                    { 4, "kryma@gmail.com", "Krystyna", "Mazurska", "Kryma", "444555666", null },
                    { 5, "joanka@gmail.com", "Joanna", "Kasztan", "Kasztan", "123456789", null }
                });

            migrationBuilder.InsertData(
                table: "Password",
                columns: new[] { "IdPassword", "IdClient", "PasswordHash" },
                values: new object[,]
                {
                    { 1, 1, "1m0FRQ9+ADXA6J1D8zGW2iPd0ihZ98W4y2frRmsrFko=" },
                    { 2, 2, "LcaOWU6xJ8499eyDCQKZ2qRI2z5LttY8M7neHdqC3JI=" },
                    { 3, 3, "WLG1enMkUiYPd7NU2n9U5bX34MkD75zeL+C8jLWv3ns=" },
                    { 4, 4, "qbaUwDkkW/hwzOv/wJ3ggn0Pqw2kSO70wtJm8YF00Yg=" },
                    { 5, 5, "A0kwYP4UXEYsPVey6//t2TDstq54GVk/11mKiwrH8ws=" }
                });

            migrationBuilder.InsertData(
                table: "Salt",
                columns: new[] { "IdSalt", "IdClient", "SaltHash" },
                values: new object[,]
                {
                    { 1, 1, "WM7C8Z+cKRVH6mnhgkXaKw==" },
                    { 2, 2, "iuwFighj2ebn6Mujq3fBww==" },
                    { 3, 3, "nPXnW/suIlZs9/S6JGXExA==" },
                    { 4, 4, "hHxIRha3dWuemYFwXXmS6Q==" },
                    { 5, 5, "bAHpe+2ORAdlmt2Q3cuKbA==" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banner_IdCampaign",
                table: "Banner",
                column: "IdCampaign");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_FromIdBuilding",
                table: "Campaign",
                column: "FromIdBuilding");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_IdClient",
                table: "Campaign",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_ToIdBuilding",
                table: "Campaign",
                column: "ToIdBuilding");

            migrationBuilder.CreateIndex(
                name: "IX_Password_IdClient",
                table: "Password",
                column: "IdClient",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salt_IdClient",
                table: "Salt",
                column: "IdClient",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banner");

            migrationBuilder.DropTable(
                name: "Password");

            migrationBuilder.DropTable(
                name: "Salt");

            migrationBuilder.DropTable(
                name: "Campaign");

            migrationBuilder.DropTable(
                name: "Building");

            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
