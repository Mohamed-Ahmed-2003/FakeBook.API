using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fakebook.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SignalR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ChatRooms");

            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActive",
                table: "UserProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "ChatMessage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "ChatMessage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "LastActive",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "ChatMessage");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "ChatMessage");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ChatRooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
