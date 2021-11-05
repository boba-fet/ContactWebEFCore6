using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyContactManagerData.Migrations
{
    public partial class addstatesandcontacts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PhonePrimary = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PhoneSecondary = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StreetAddress1 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    StreetAddress2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Abbreviation", "Name" },
                values: new object[,]
                {
                    { 1, "AL", "Alabama" },
                    { 2, "AK", "Alaska" },
                    { 3, "AZ", "Arizona" },
                    { 4, "AR", "Arkansas" },
                    { 5, "CA", "California" },
                    { 6, "CO", "Colorado" },
                    { 7, "CT", "Connecticut" },
                    { 8, "DE", "Delaware" },
                    { 9, "DC", "District of Columbia" },
                    { 10, "FL", "Florida" },
                    { 11, "GA", "Georgia" },
                    { 12, "HI", "Hawaii" },
                    { 13, "ID", "Idaho" },
                    { 14, "IL", "Illinois" },
                    { 15, "IN", "Indiana" },
                    { 16, "IA", "Iowa" },
                    { 17, "KS", "Kansas" },
                    { 18, "KY", "Kentucky" },
                    { 19, "LA", "Louisiana" },
                    { 20, "ME", "Maine" },
                    { 21, "MD", "Maryland" },
                    { 22, "MS", "Massachusetts" },
                    { 23, "MI", "Michigan" },
                    { 24, "MN", "Minnesota" },
                    { 25, "MS", "Mississippi" },
                    { 26, "MO", "Missouri" },
                    { 27, "MT", "Montana" },
                    { 28, "NE", "Nebraska" },
                    { 29, "NV", "Nevada" },
                    { 30, "NH", "New Hampshire" },
                    { 31, "NJ", "New Jersey" },
                    { 32, "NM", "New Mexico" },
                    { 33, "NY", "New York" },
                    { 34, "NC", "North Carolina" },
                    { 35, "ND", "North Dakota" },
                    { 36, "OH", "Ohio" },
                    { 37, "OK", "Oklahoma" },
                    { 38, "OR", "Oregon" },
                    { 39, "PA", "Pennsylvania" },
                    { 40, "RI", "Rhode Island" },
                    { 41, "SC", "South Carolina" },
                    { 42, "SD", "South Dakota" }
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Abbreviation", "Name" },
                values: new object[,]
                {
                    { 43, "TN", "Tennessee" },
                    { 44, "TX", "Texas" },
                    { 45, "UT", "Utah" },
                    { 46, "VT", "Vermont" },
                    { 47, "VA", "Virginia" },
                    { 48, "WA", "Washington" },
                    { 49, "WV", "West Virginia" },
                    { 50, "WI", "Wisconsin" },
                    { 51, "WY", "Wyoming" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_StateId",
                table: "Contacts",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "States");
        }
    }
}
