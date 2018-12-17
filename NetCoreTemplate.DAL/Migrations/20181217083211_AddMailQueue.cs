using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreTemplate.DAL.Migrations
{
    public partial class AddMailQueue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MailQueue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    To = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    SentOn = table.Column<DateTime>(nullable: true),
                    NumberOfTimesFailed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailQueue", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailQueue");
        }
    }
}
