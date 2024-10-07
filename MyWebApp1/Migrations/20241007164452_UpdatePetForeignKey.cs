using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebApp1.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePetForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Pet_PetCategoryId",
                table: "Pet",
                column: "PetCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_PetCategory_PetCategoryId",
                table: "Pet",
                column: "PetCategoryId",
                principalTable: "PetCategory",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pet_PetCategory_PetCategoryId",
                table: "Pet");

            migrationBuilder.DropIndex(
                name: "IX_Pet_PetCategoryId",
                table: "Pet");
        }
    }
}
