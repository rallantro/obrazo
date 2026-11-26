using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace obrazo.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueMediaNameAndType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Medias_Name_Type",
                table: "Medias",
                columns: new[] { "Name", "Type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Medias_Name_Type",
                table: "Medias");
        }
    }
}
