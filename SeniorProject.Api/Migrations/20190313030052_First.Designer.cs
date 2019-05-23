﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SeniorProject.Api;

namespace SeniorProject.Api.Migrations
{
    [DbContext(typeof(ShoppingAssistantAPIContext))]
    [Migration("20190313030052_First")]
    partial class First
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.AddressEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("Latitude")
                        .IsRequired();

                    b.Property<string>("Longitude")
                        .IsRequired();

                    b.Property<string>("State")
                        .IsRequired();

                    b.Property<string>("Street")
                        .IsRequired();

                    b.Property<string>("Zip")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.AisleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Column");

                    b.Property<int?>("LowerDepartmentEntityId");

                    b.Property<int>("LowerDepartmenttId");

                    b.Property<string>("Name");

                    b.Property<int>("Row");

                    b.Property<string>("SideOfAisle");

                    b.HasKey("Id");

                    b.HasIndex("LowerDepartmentEntityId");

                    b.ToTable("Aisles");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.BaseUserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<DateTimeOffset>("TimeOfCreation");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BaseUserEntity");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.DepartmentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColumnsDefinitions");

                    b.Property<string>("MapLocation");

                    b.Property<string>("Name");

                    b.Property<string>("RowDefinitions");

                    b.Property<int?>("StoreMapEntityId");

                    b.Property<int>("StoreMapId");

                    b.HasKey("Id");

                    b.HasIndex("StoreMapEntityId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.ItemEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("SpoonacularProductId");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.ItemShoppingListLinkEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ItemId");

                    b.Property<int>("ItemQuantity");

                    b.Property<int>("ShoppingListId");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ShoppingListId");

                    b.ToTable("ItemShoppingListLinks");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.ItemStoreLinkEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AisleId");

                    b.Property<int>("DepartmentId");

                    b.Property<bool>("InStock");

                    b.Property<int>("ItemId");

                    b.Property<int>("LowerDepartmentId");

                    b.Property<decimal>("Price");

                    b.Property<int>("SectionId");

                    b.Property<int>("ShelfId");

                    b.Property<int>("SlotId");

                    b.Property<int>("StockAmount");

                    b.Property<int>("StoreId");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("StoreId");

                    b.ToTable("ItemStoreLinks");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.LowerDepartmentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AisleCount");

                    b.Property<int>("AisleStart");

                    b.Property<int>("Column");

                    b.Property<int?>("DepartmentEntityId");

                    b.Property<int>("DepartmentId");

                    b.Property<string>("Name");

                    b.Property<int>("Row");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentEntityId");

                    b.ToTable("LowerDepartments");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.SectionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AisleEntityId");

                    b.Property<int>("AisleId");

                    b.Property<int>("ItemsPerShelf");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AisleEntityId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.ShelfEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("SectionEntityId");

                    b.Property<int>("SectionId");

                    b.Property<int>("ShelfNumber");

                    b.HasKey("Id");

                    b.HasIndex("SectionEntityId");

                    b.ToTable("Shelfs");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.ShelfSlotEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ItemId");

                    b.Property<int?>("ShelfEntityId");

                    b.Property<int>("ShelfId");

                    b.Property<int>("SlotOnShelf");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ShelfEntityId");

                    b.ToTable("ShelfSlots");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.ShoppingListEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<Guid?>("ShoppingUserEntityId");

                    b.Property<Guid>("ShoppingUserId");

                    b.Property<int>("StoreId");

                    b.Property<DateTimeOffset>("TimeOfCreation");

                    b.HasKey("Id");

                    b.HasIndex("ShoppingUserEntityId");

                    b.ToTable("ShoppingLists");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.StoreEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AddressId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<int>("StoreMapId");

                    b.Property<Guid>("StoreUserId");

                    b.Property<string>("Website")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.StoreMapEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColumnsDefinitions");

                    b.Property<string>("RowDefinitions");

                    b.HasKey("Id");

                    b.ToTable("StoreMaps");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.UserRoleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.AdminUserEntity", b =>
                {
                    b.HasBaseType("SeniorProject.Api.Models.Entities.BaseUserEntity");


                    b.ToTable("AdminUserEntity");

                    b.HasDiscriminator().HasValue("AdminUserEntity");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.ShoppingUserEntity", b =>
                {
                    b.HasBaseType("SeniorProject.Api.Models.Entities.BaseUserEntity");

                    b.Property<int>("HomeStoreId");

                    b.ToTable("ShoppingUserEntity");

                    b.HasDiscriminator().HasValue("ShoppingUserEntity");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.StoreUserEntity", b =>
                {
                    b.HasBaseType("SeniorProject.Api.Models.Entities.BaseUserEntity");

                    b.Property<int>("HomeStoreId")
                        .HasColumnName("StoreUserEntity_HomeStoreId");

                    b.ToTable("StoreUserEntity");

                    b.HasDiscriminator().HasValue("StoreUserEntity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.UserRoleEntity")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.BaseUserEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.BaseUserEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.UserRoleEntity")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SeniorProject.Api.Models.Entities.BaseUserEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.BaseUserEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.AisleEntity", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.LowerDepartmentEntity")
                        .WithMany("Aisles")
                        .HasForeignKey("LowerDepartmentEntityId");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.DepartmentEntity", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.StoreMapEntity")
                        .WithMany("Departments")
                        .HasForeignKey("StoreMapEntityId");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.ItemShoppingListLinkEntity", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.ItemEntity", "Item")
                        .WithMany("ItemListLinks")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SeniorProject.Api.Models.Entities.ShoppingListEntity", "ShoppingList")
                        .WithMany("ListItemLinks")
                        .HasForeignKey("ShoppingListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.ItemStoreLinkEntity", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.ItemEntity", "Item")
                        .WithMany("ItemStoreLinks")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SeniorProject.Api.Models.Entities.StoreEntity", "Store")
                        .WithMany("ItemStoreLinks")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.LowerDepartmentEntity", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.DepartmentEntity")
                        .WithMany("LowerDepartments")
                        .HasForeignKey("DepartmentEntityId");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.SectionEntity", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.AisleEntity")
                        .WithMany("Sections")
                        .HasForeignKey("AisleEntityId");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.ShelfEntity", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.SectionEntity")
                        .WithMany("Shelves")
                        .HasForeignKey("SectionEntityId");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.ShelfSlotEntity", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.ItemEntity", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SeniorProject.Api.Models.Entities.ShelfEntity")
                        .WithMany("Slots")
                        .HasForeignKey("ShelfEntityId");
                });

            modelBuilder.Entity("SeniorProject.Api.Models.Entities.ShoppingListEntity", b =>
                {
                    b.HasOne("SeniorProject.Api.Models.Entities.ShoppingUserEntity")
                        .WithMany("ShoppingLists")
                        .HasForeignKey("ShoppingUserEntityId");
                });
#pragma warning restore 612, 618
        }
    }
}
