using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fakebook.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Post_Delete_Cascade_comments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_UserProfiles_UserProfileId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_UserProfiles_UserProfileId",
                table: "Posts",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_UserProfiles_UserProfileId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_UserProfiles_UserProfileId",
                table: "Posts",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
