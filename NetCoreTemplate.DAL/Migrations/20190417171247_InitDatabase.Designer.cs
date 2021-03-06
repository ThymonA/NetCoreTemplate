﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetCoreTemplate.DAL;

namespace NetCoreTemplate.DAL.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20190417171247_InitDatabase")]
    partial class InitDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.FileManager.FileManagerDirectory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created_On");

                    b.Property<int?>("FileManagerDirectory_Id");

                    b.Property<int?>("Identifier");

                    b.Property<string>("Location");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.Property<DateTime?>("Updated_On");

                    b.Property<int>("User_Id");

                    b.HasKey("Id");

                    b.HasIndex("FileManagerDirectory_Id");

                    b.HasIndex("User_Id");

                    b.ToTable("FileManagerDirectory");
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.FileManager.FileManagerFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatedBy_User_Id");

                    b.Property<DateTimeOffset>("Created_On");

                    b.Property<string>("Extension");

                    b.Property<int>("FileManagerDirectory_Id");

                    b.Property<string>("Guid");

                    b.Property<int?>("Identifier");

                    b.Property<string>("Location");

                    b.Property<string>("MimeType");

                    b.Property<string>("Name");

                    b.Property<long>("Size");

                    b.Property<DateTimeOffset?>("Updated_On");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy_User_Id");

                    b.HasIndex("FileManagerDirectory_Id");

                    b.ToTable("FileManagerFile");
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.General.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("CultureCode")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Language");
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.General.MailQueue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedOn");

                    b.Property<string>("Content");

                    b.Property<int>("NumberOfTimesFailed");

                    b.Property<DateTime?>("SentOn");

                    b.Property<string>("Subject");

                    b.Property<string>("To")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("MailQueue");
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.General.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action")
                        .IsRequired();

                    b.Property<int>("EntityLabelDefinition_Id");

                    b.HasKey("Id");

                    b.HasIndex("EntityLabelDefinition_Id");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.General.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.General.RolePermission", b =>
                {
                    b.Property<int>("Permission_Id");

                    b.Property<int>("Role_Id");

                    b.HasKey("Permission_Id", "Role_Id");

                    b.HasIndex("Role_Id");

                    b.ToTable("RolePermission");
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.General.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Firstname");

                    b.Property<string>("Lastname");

                    b.Property<string>("Password");

                    b.Property<string>("ResetToken");

                    b.Property<DateTime?>("ResetTokenDate");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.General.UserRole", b =>
                {
                    b.Property<int>("Role_Id");

                    b.Property<int>("User_Id");

                    b.HasKey("Role_Id", "User_Id");

                    b.HasIndex("User_Id");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.Translation.EntityLabel", b =>
                {
                    b.Property<int>("EntityLabelDefinition_Id");

                    b.Property<int>("Language_Id");

                    b.Property<string>("Label")
                        .IsRequired();

                    b.HasKey("EntityLabelDefinition_Id", "Language_Id");

                    b.HasIndex("Language_Id");

                    b.ToTable("EntityLabel");
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.Translation.EntityLabelDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("EntityLabelDefinition");
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.Translation.TranslationLabel", b =>
                {
                    b.Property<int>("Language_Id");

                    b.Property<int>("TranslationLabelDefinition_Id");

                    b.Property<string>("Label")
                        .IsRequired();

                    b.HasKey("Language_Id", "TranslationLabelDefinition_Id");

                    b.HasIndex("TranslationLabelDefinition_Id");

                    b.ToTable("TranslationLabel");
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.Translation.TranslationLabelDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Module")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("TranslationLabelDefinition");
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.FileManager.FileManagerDirectory", b =>
                {
                    b.HasOne("NetCoreTemplate.DAL.Models.FileManager.FileManagerDirectory", "Parent")
                        .WithMany("FileManagerDirectories")
                        .HasForeignKey("FileManagerDirectory_Id");

                    b.HasOne("NetCoreTemplate.DAL.Models.General.User", "CreatedUser")
                        .WithMany("FileManagerDirectories")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.FileManager.FileManagerFile", b =>
                {
                    b.HasOne("NetCoreTemplate.DAL.Models.General.User", "User")
                        .WithMany()
                        .HasForeignKey("CreatedBy_User_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NetCoreTemplate.DAL.Models.FileManager.FileManagerDirectory", "FileManagerDirectory")
                        .WithMany()
                        .HasForeignKey("FileManagerDirectory_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.General.Permission", b =>
                {
                    b.HasOne("NetCoreTemplate.DAL.Models.Translation.EntityLabelDefinition", "EntityLabelDefinition")
                        .WithMany()
                        .HasForeignKey("EntityLabelDefinition_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.General.RolePermission", b =>
                {
                    b.HasOne("NetCoreTemplate.DAL.Models.General.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("Permission_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NetCoreTemplate.DAL.Models.General.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("Role_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.General.UserRole", b =>
                {
                    b.HasOne("NetCoreTemplate.DAL.Models.General.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("Role_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NetCoreTemplate.DAL.Models.General.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.Translation.EntityLabel", b =>
                {
                    b.HasOne("NetCoreTemplate.DAL.Models.Translation.EntityLabelDefinition", "EntityLabelDefinition")
                        .WithMany("EntityLabels")
                        .HasForeignKey("EntityLabelDefinition_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NetCoreTemplate.DAL.Models.General.Language", "Language")
                        .WithMany("EntityLabels")
                        .HasForeignKey("Language_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetCoreTemplate.DAL.Models.Translation.TranslationLabel", b =>
                {
                    b.HasOne("NetCoreTemplate.DAL.Models.General.Language", "Language")
                        .WithMany("TranslationLabels")
                        .HasForeignKey("Language_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NetCoreTemplate.DAL.Models.Translation.TranslationLabelDefinition", "TranslationLabelDefinition")
                        .WithMany("TranslationLabels")
                        .HasForeignKey("TranslationLabelDefinition_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
