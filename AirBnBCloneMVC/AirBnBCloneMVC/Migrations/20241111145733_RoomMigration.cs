using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirBnBCloneMVC.Migrations
{
    /// <inheritdoc />
    public partial class RoomMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Owner_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Room_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Room_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Room_Capacity = table.Column<int>(type: "int", nullable: false),
                    Room_Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Room_PricePerNight = table.Column<double>(type: "float", nullable: false),
                    Room_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Has_Wifi = table.Column<bool>(type: "bit", nullable: false),
                    Has_Pool = table.Column<bool>(type: "bit", nullable: false),
                    Has_Kitchen = table.Column<bool>(type: "bit", nullable: false),
                    Has_Parking = table.Column<bool>(type: "bit", nullable: false),
                    Has_AirConditioning = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);

                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
