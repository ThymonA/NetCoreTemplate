using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreTemplate.DAL.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityLabelDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityLabelDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    CultureCode = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MailQueue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    To = table.Column<string>(nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TranslationLabelDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Module = table.Column<string>(maxLength: 128, nullable: false),
                    Type = table.Column<string>(maxLength: 128, nullable: false),
                    Key = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationLabelDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Firstname = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    ResetToken = table.Column<string>(nullable: true),
                    ResetTokenDate = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntityLabelDefinition_Id = table.Column<int>(nullable: false),
                    Action = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_EntityLabelDefinition_EntityLabelDefinition_Id",
                        column: x => x.EntityLabelDefinition_Id,
                        principalTable: "EntityLabelDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityLabel",
                columns: table => new
                {
                    EntityLabelDefinition_Id = table.Column<int>(nullable: false),
                    Language_Id = table.Column<int>(nullable: false),
                    Label = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityLabel", x => new { x.EntityLabelDefinition_Id, x.Language_Id });
                    table.ForeignKey(
                        name: "FK_EntityLabel_EntityLabelDefinition_EntityLabelDefinition_Id",
                        column: x => x.EntityLabelDefinition_Id,
                        principalTable: "EntityLabelDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityLabel_Language_Language_Id",
                        column: x => x.Language_Id,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TranslationLabel",
                columns: table => new
                {
                    TranslationLabelDefinition_Id = table.Column<int>(nullable: false),
                    Language_Id = table.Column<int>(nullable: false),
                    Label = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationLabel", x => new { x.Language_Id, x.TranslationLabelDefinition_Id });
                    table.ForeignKey(
                        name: "FK_TranslationLabel_Language_Language_Id",
                        column: x => x.Language_Id,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TranslationLabel_TranslationLabelDefinition_TranslationLabel~",
                        column: x => x.TranslationLabelDefinition_Id,
                        principalTable: "TranslationLabelDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileManagerDirectory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    FileManagerDirectory_Id = table.Column<int>(nullable: true),
                    User_Id = table.Column<int>(nullable: false),
                    Identifier = table.Column<int>(nullable: true),
                    Created_On = table.Column<DateTime>(nullable: false),
                    Updated_On = table.Column<DateTime>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileManagerDirectory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileManagerDirectory_FileManagerDirectory_FileManagerDirecto~",
                        column: x => x.FileManagerDirectory_Id,
                        principalTable: "FileManagerDirectory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileManagerDirectory_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    User_Id = table.Column<int>(nullable: false),
                    Role_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.Role_Id, x.User_Id });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Role_Id = table.Column<int>(nullable: false),
                    Permission_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.Permission_Id, x.Role_Id });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_Permission_Id",
                        column: x => x.Permission_Id,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileManagerFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Guid = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    MimeType = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    FileManagerDirectory_Id = table.Column<int>(nullable: false),
                    CreatedBy_User_Id = table.Column<int>(nullable: false),
                    Identifier = table.Column<int>(nullable: true),
                    Created_On = table.Column<DateTimeOffset>(nullable: false),
                    Updated_On = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileManagerFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileManagerFile_User_CreatedBy_User_Id",
                        column: x => x.CreatedBy_User_Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileManagerFile_FileManagerDirectory_FileManagerDirectory_Id",
                        column: x => x.FileManagerDirectory_Id,
                        principalTable: "FileManagerDirectory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityLabel_Language_Id",
                table: "EntityLabel",
                column: "Language_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagerDirectory_FileManagerDirectory_Id",
                table: "FileManagerDirectory",
                column: "FileManagerDirectory_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagerDirectory_User_Id",
                table: "FileManagerDirectory",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagerFile_CreatedBy_User_Id",
                table: "FileManagerFile",
                column: "CreatedBy_User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FileManagerFile_FileManagerDirectory_Id",
                table: "FileManagerFile",
                column: "FileManagerDirectory_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_EntityLabelDefinition_Id",
                table: "Permission",
                column: "EntityLabelDefinition_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_Role_Id",
                table: "RolePermission",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TranslationLabel_TranslationLabelDefinition_Id",
                table: "TranslationLabel",
                column: "TranslationLabelDefinition_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_User_Id",
                table: "UserRole",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityLabel");

            migrationBuilder.DropTable(
                name: "FileManagerFile");

            migrationBuilder.DropTable(
                name: "MailQueue");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "TranslationLabel");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "FileManagerDirectory");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "TranslationLabelDefinition");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "EntityLabelDefinition");
        }
    }
}
