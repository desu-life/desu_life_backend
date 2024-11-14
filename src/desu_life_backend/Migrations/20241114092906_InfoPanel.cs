using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace desu.life.Migrations
{
    /// <inheritdoc />
    public partial class InfoPanel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfoPanel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "主键")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "Panel标题")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: false, comment: "Panel HTML内容")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Audited = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否审核通过"),
                    Applied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否已应用"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "修改时间"),
                    AuthorId = table.Column<int>(type: "int", nullable: false, comment: "创建者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoPanel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoPanel_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InfoPanel_AuthorId",
                table: "InfoPanel",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfoPanel");
        }
    }
}
