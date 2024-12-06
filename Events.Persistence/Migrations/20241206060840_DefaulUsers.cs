using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Events.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DefaulUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "Description", "Name" },
                values: new object[] { "123 Elm Street, Springfield, IL 62704, USA", "Join our workshop to learn about modern digital marketing strategies. Get hands-on experience with practical tasks and expert tips.", "Digital Marketing Workshop" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "Description", "Name" },
                values: new object[] { "456 Maple Avenue, Los Angeles, CA 90001, USA", "Don’t miss a live performance by a local rock band at a cozy bar! Great atmosphere and fantastic music guaranteed.", " Local Band Concert" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Address", "Description", "Name" },
                values: new object[] { "789 Oak Lane, New York, NY 10001, USA", "Come to the street food festival and enjoy a variety of dishes from the best local chefs. Great food and live music await you!", "Street Food Festival" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "Email", "Name", "Password", "RegistrationDate", "Role", "Surname", "Token" },
                values: new object[,]
                {
                    { 1, new DateOnly(1999, 12, 12), "admin1@mail.com", "Admin", "Admin123!", new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Admin", null },
                    { 2, new DateOnly(1999, 12, 12), "user1@mail.com", "User", "User12345!", new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "User", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "Description", "Name" },
                values: new object[] { "adr1", "desc", "name" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "Description", "Name" },
                values: new object[] { "adr2", "desc2", "name2" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Address", "Description", "Name" },
                values: new object[] { "adr3", "desc3", "name3" });
        }
    }
}
