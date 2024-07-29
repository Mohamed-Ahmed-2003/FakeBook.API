using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fakebook.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Post_Delete_Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Posts_PostId",
                table: "Media");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Posts_PostId",
                table: "Media",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Posts_PostId",
                table: "Media");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Posts_PostId",
                table: "Media",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");
        }
    }
}
