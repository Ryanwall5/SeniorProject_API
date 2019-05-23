using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SeniorProject.Api.Models;
using SeniorProject.Api.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api
{
    public class ShoppingAssistantAPIContext : IdentityDbContext<BaseUserEntity, UserRoleEntity, Guid>
    {
        public ShoppingAssistantAPIContext(DbContextOptions<ShoppingAssistantAPIContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BaseUserEntity>().Property(p => p.Id).ValueGeneratedOnAdd();

            //builder.Entity<ItemShoppingListLinkEntity>().HasKey(isl => new { isl.ItemId, isl.ShoppingListId });
            //builder.Entity<ItemStoreLinkEntity>().HasKey(isl => new { isl.ItemId, isl.StoreId });
        }


        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<AdminUserEntity> AdminUsers { get; set; }
        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<ItemShoppingListLinkEntity> ItemShoppingListLinks { get; set; }
        public DbSet<ItemStoreLinkEntity> ItemStoreLinks { get; set; }
        public DbSet<ShoppingListEntity> ShoppingLists { get; set; }
        public DbSet<ShoppingUserEntity> ShoppingUsers { get; set; }
        public DbSet<StoreUserEntity> StoreUsers { get; set; }
        public DbSet<StoreEntity> Stores { get; set; }

        public DbSet<StoreMapEntity> StoreMaps { get; set; }
        public DbSet<DepartmentEntity> Departments { get; set; }
        public DbSet<LowerDepartmentEntity> LowerDepartments { get; set; }
        public DbSet<AisleEntity> Aisles { get; set; }
        public DbSet<SectionEntity> Sections { get; set; }
        public DbSet<ShelfEntity> Shelfs { get; set; }
        public DbSet<ShelfSlotEntity> ShelfSlots { get; set; }
    }


    public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<ShoppingAssistantAPIContext>
    {
        ShoppingAssistantAPIContext IDesignTimeDbContextFactory<ShoppingAssistantAPIContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShoppingAssistantAPIContext>();
            optionsBuilder.UseSqlServer(@"Server=tcp:shopping-assistant-server.database.windows.net,1433;Initial Catalog=ShoppingAssistantDB;Persist Security Info=False;User ID=Ryanwall5;Password=Beavers522!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            return new ShoppingAssistantAPIContext(optionsBuilder.Options);
        }
    }
}
