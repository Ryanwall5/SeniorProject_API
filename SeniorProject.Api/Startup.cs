using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SeniorProject.Api.Infrastructure;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorProject.Api
{
    public class Startup
    {

        private readonly int? HttpsPort;

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)

                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Use an in memory database
            //TODO: make real database  
            services.AddDbContext<ShoppingAssistantAPIContext>(opt => opt.UseSqlServer(@"Server=tcp:shopping-assistant-server.database.windows.net,1433;Initial Catalog=ShoppingAssistantDB;Persist Security Info=False;User ID=Ryanwall5;Password=Beavers522!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Add ASP.NET Core Identity
            //services.AddIdentity<StoreUserEntity, UserRoleEntity>()
            //    .AddEntityFrameworkStores<ShoppingAssistantAPIContext>();
            //services.AddIdentity<ShoppingUserEntity, UserRoleEntity>()
            //    .AddEntityFrameworkStores<ShoppingAssistantAPIContext>();
            services.AddIdentity<BaseUserEntity, UserRoleEntity>()
                .AddEntityFrameworkStores<ShoppingAssistantAPIContext>();

            var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Shopping Assistant Secret Code"));
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {

                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = signKey,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };

            });

            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(RequireHttpsAttribute));

                // opt.Filters.Add(typeof(JsonExceptionFilter));
            });
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>()
                .ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddScoped<IShoppingUserRepository, ShoppingUserRepository>();
            services.AddScoped<IStoreUserRepository, StoreUserRepository>();
            services.AddScoped<IItemStoreLinkRepository, ItemStoreLinkRepository>();
            services.AddScoped<IItemShoppingListLinkRepository, ItemShoppingListLinkRepository>();
            services.AddScoped<IShoppingListRepository, ShoppingListRepository>();
            services.AddScoped<IItemEntityRepository, ItemRepository>();

            services.AddScoped<IRepository<AdminUserEntity>, AdminUserRepository>();
            services.AddScoped<IRepository<BaseUserEntity>, BaseUserRepository>();

            services.AddScoped<IRepository<StoreEntity>, StoreRepository>();
            services.AddScoped<IRepository<AddressEntity>, AddressRepository>();
            services.AddScoped<IRepository<StoreMapEntity>, StoreMapRepository>();
            services.AddScoped<IRepository<DepartmentEntity>, DepartmentRepository>();
            services.AddScoped<ILowerDepartmentRepository, LowerDepartmentRepository>();
            services.AddScoped<IAisleRepository, AisleRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<IShelfRepository, ShelfRepository>();
            services.AddScoped<IShelfSlotsRepository, ShelfSlotRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {

                var roleManager = app.ApplicationServices.GetRequiredService<RoleManager<UserRoleEntity>>();
                var userManager = app.ApplicationServices.GetRequiredService<UserManager<BaseUserEntity>>();

                var context = app.ApplicationServices.GetRequiredService<ShoppingAssistantAPIContext>();

                // ADD THIS WHEN DELETE MIGRATIONS 
                // TODO: Add more functions here to add more test data
                //AddRoles(roleManager).Wait();
                //AddTestUsers(roleManager, userManager).Wait();
                //AddAddress(context);
                //AddStores(context);
                //AddItems(context);
                //AddStoreMapStuff(context);
                //AddItemsToStore(context);
                //AddShoppingList(context);
                //AddShoppingItems(context);
                //AddInMemoryData(context);
                //AddStoreMap(context);
                //AddAisles(context);
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }


        #region Refill database after deleting database

        private static async void AddAisles(ShoppingAssistantAPIContext context)
        {
            LowerDepartmentEntity lowerd = await context.LowerDepartments.FirstOrDefaultAsync(ld => ld.Id == 1);

            List<AisleEntity> aislesCanned = new List<AisleEntity>
            {
                new AisleEntity
                {
                    LowerDepartmenttId = lowerd.Id,
                    Name = "1",
                    SideOfAisle = "Right",
                    Row = 1,
                    Column = 1,
                    Sections = new List<SectionEntity>()
                },
                new AisleEntity
                {
                    LowerDepartmenttId = lowerd.Id,
                    Name = "2",
                    SideOfAisle = "left",
                    Row = 1,
                    Column = 3,
                    Sections = new List<SectionEntity>()
                },
                new AisleEntity
                {
                    LowerDepartmenttId = lowerd.Id,
                    Name = "2",
                    SideOfAisle = "right",
                    Row = 1,
                    Column = 5,
                    Sections = new List<SectionEntity>()
                },
                new AisleEntity
                {
                    LowerDepartmenttId = lowerd.Id,
                    Name = "3",
                    SideOfAisle = "left",
                    Row = 1,
                    Column = 7,
                    Sections = new List<SectionEntity>()
                },
                new AisleEntity
                {
                    LowerDepartmenttId = lowerd.Id,
                    Name = "3",
                    SideOfAisle = "right",
                    Row = 1,
                    Column = 9,
                    Sections = new List<SectionEntity>()
                },
                 new AisleEntity
                {
                     LowerDepartmenttId = lowerd.Id,
                    Name = "4",
                    SideOfAisle = "left",
                    Row = 1,
                    Column = 11,
                    Sections = new List<SectionEntity>()
                },
            };

            foreach (AisleEntity a in aislesCanned)
            {
                context.Aisles.Add(a);
            }

            context.SaveChanges();
        }


        private static async Task AddRoles(RoleManager<UserRoleEntity> roleManager)
        {
            // Add User Roles
            await roleManager.CreateAsync(new UserRoleEntity("Shopping"));
            await roleManager.CreateAsync(new UserRoleEntity("Admin"));
            await roleManager.CreateAsync(new UserRoleEntity("Store"));
        }

        private static async Task AddTestUsers(RoleManager<UserRoleEntity> roleManager, UserManager<BaseUserEntity> userManager)
        {
            // Add User Roles
            await roleManager.CreateAsync(new UserRoleEntity("Shopping"));
            await roleManager.CreateAsync(new UserRoleEntity("Admin"));
            await roleManager.CreateAsync(new UserRoleEntity("Store"));

            // Create Users
            var shoppingUser = new ShoppingUserEntity
            {
                Email = "testshopper@SeniorProject.local",
                UserName = "testshopper@SeniorProject.local",
                FirstName = "Shopper",
                LastName = "Tester",
                HomeStoreId = 1,
                TimeOfCreation = DateTimeOffset.UtcNow
            };

            var adminUser = new AdminUserEntity
            {
                Email = "testadmin@SeniorProject.local",
                UserName = "testadmin@SeniorProject.local",
                FirstName = "Admin",
                LastName = "Tester",
                TimeOfCreation = DateTimeOffset.UtcNow
            };

            var storeUser1 = new StoreUserEntity
            {
                Email = "teststore1@SeniorProject.local",
                UserName = "teststore1@SeniorProject.local",
                FirstName = "StoreUser1",
                LastName = "Tester",
                HomeStoreId = 1,
                TimeOfCreation = DateTimeOffset.UtcNow
            };
            var storeUser2 = new StoreUserEntity
            {
                Email = "teststore2@SeniorProject.local",
                UserName = "teststore2@SeniorProject.local",
                FirstName = "StoreUser2",
                LastName = "Tester",
                HomeStoreId = 1,
                TimeOfCreation = DateTimeOffset.UtcNow
            };

            // Create users
            await userManager.CreateAsync(shoppingUser, "Shopper123!");
            await userManager.CreateAsync(adminUser, "Admin123!");
            await userManager.CreateAsync(storeUser1, "Store123!");
            await userManager.CreateAsync(storeUser2, "Store123!");

            // Add users to roles
            await userManager.AddToRoleAsync(shoppingUser, "Shopping");
            await userManager.AddToRoleAsync(adminUser, "Admin");
            await userManager.AddToRoleAsync(storeUser1, "Store");
            await userManager.AddToRoleAsync(storeUser2, "Store");

            //Update the userManager
            await userManager.UpdateAsync(shoppingUser);
            await userManager.UpdateAsync(adminUser);
            await userManager.UpdateAsync(storeUser1);
            await userManager.UpdateAsync(storeUser2);
        }


        private static void AddAddress(ShoppingAssistantAPIContext context)
        {
            List<AddressEntity> addresses = new List<AddressEntity>
            {
                new AddressEntity
                {
                    Street = "1041 SE 1st Ave",
                    City = "Canby",
                    State = "Oregon",
                    Zip = "97013",
                    Longitude = "-122.6767",
                    Latitude = "45.2664"
                },
                new AddressEntity
                {
                    Street = "1051 SW 1st Ave",
                    City = "Canby",
                    State = "Oregon",
                    Zip = "97013",
                    Longitude = "-122.7026",
                    Latitude = "45.2554"
                }
            };

            foreach (AddressEntity address in addresses)
            {
                context.Addresses.Add(address);
            }
            context.SaveChanges();
        }

        private static void AddStores(ShoppingAssistantAPIContext context)
        {
            var user1 = context.StoreUsers.FirstOrDefault(u => u.FirstName == "StoreUser1");
            var user2 = context.StoreUsers.FirstOrDefault(u => u.FirstName == "StoreUser2");

            List<StoreEntity> stores = new List<StoreEntity>
            {
                new StoreEntity
                {
                    AddressId = 1,
                    Name = "Fred Meyer",
                    PhoneNumber = "5032634100",
                    Website = "www.fredmeyer.com",
                    StoreUserId = user1.Id,
                },
                new StoreEntity
                {
                    AddressId = 2,
                    Name = "Safeway",
                    PhoneNumber = "5032665890",
                    Website = "www.safeway.com",
                    StoreUserId = user2.Id
                }
            };

            foreach(StoreEntity store in stores)
            {
                context.Stores.Add(store);
            }

            context.SaveChanges();
        }

        private static void AddShoppingList(ShoppingAssistantAPIContext context)
        {
            var shoppingUser = context.ShoppingUsers.First();
            List<ShoppingListEntity> shoppingLists = new List<ShoppingListEntity>
            {
                new ShoppingListEntity
                {
                    Name = "Holidays",
                    TimeOfCreation = DateTime.Now,
                    ShoppingUserId = shoppingUser.Id,
                    StoreId = 1
                },
                new ShoppingListEntity
                {
                    Name = "Thanks Giving",
                    TimeOfCreation = DateTime.Now,
                    ShoppingUserId = shoppingUser.Id,
                    StoreId = 1
                },
                new ShoppingListEntity
                {
                    Name = "Christmas",
                    TimeOfCreation = DateTime.Now,
                    ShoppingUserId = shoppingUser.Id,
                    StoreId = 1
                },
                new ShoppingListEntity
                {
                    Name = "Weekly",
                    TimeOfCreation = DateTime.Now,
                    ShoppingUserId = shoppingUser.Id,
                    StoreId = 1
                }
            };

            foreach(ShoppingListEntity list in shoppingLists)
            {
                context.ShoppingLists.Add(list);
            }
            context.SaveChanges();
        }

        private void AddStoreMap(ShoppingAssistantAPIContext context)
        {

            #region StoreMap

            var departments = context.Departments.ToList();

            StoreMapEntity storeMap = new StoreMapEntity()
            {
                Departments = departments,
                ColumnsDefinitions = "60,70,*,*,*",
                RowDefinitions = "*,*,*,*,*,*,*",
            };

            context.StoreMaps.Add(storeMap);

            context.SaveChanges();

            var store = context.Stores.First();
            store.StoreMapId = storeMap.Id;

            context.Stores.Update(store);
            context.SaveChanges();

            #endregion
        }

        private static void AddStoreMapStuff(ShoppingAssistantAPIContext context)
        {

            var items = context.Items.Take(27).ToArray();

            #region ShelfSlots

            List<ShelfSlotEntity> msl1 = new List<ShelfSlotEntity>
            {
                //First Shelf
                new ShelfSlotEntity
                {
                    SlotOnShelf = 1,
                    Item = items[0],
                    ItemId = items[0].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 2,
                    Item = items[1],
                    ItemId = items[1].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 3,
                    Item = items[2],
                    ItemId = items[2].Id,
                },
                //Second Shelf
                new ShelfSlotEntity
                {
                    SlotOnShelf = 1,
                    Item = items[3],
                    ItemId = items[3].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 2,
                    Item = items[4],
                    ItemId = items[4].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 3,
                    Item = items[5],
                    ItemId = items[5].Id,
                },
                //third shelf
                new ShelfSlotEntity
                {
                    SlotOnShelf = 1,
                    Item = items[6],
                    ItemId = items[6].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 2,
                    Item = items[7],
                    ItemId = items[7].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 3,
                    Item = items[8],
                    ItemId = items[8].Id,
                },
            };

            foreach (ShelfSlotEntity sl in msl1)
            {
                context.ShelfSlots.Add(sl);
            }
            context.SaveChanges();

            List<ShelfSlotEntity> msl2 = new List<ShelfSlotEntity>
            {
                //First Shelf
                new ShelfSlotEntity
                {
                    SlotOnShelf = 1,
                    Item = items[9],
                    ItemId = items[9].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 2,
                    Item = items[10],
                    ItemId = items[10].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 3,
                    Item = items[11],
                    ItemId = items[11].Id,
                },
                //Second Shelf
                new ShelfSlotEntity
                {
                    SlotOnShelf = 1,
                    Item = items[12],
                    ItemId = items[12].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 2,
                    Item = items[13],
                    ItemId = items[13].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 3,
                    Item = items[14],
                    ItemId = items[14].Id,
                },
                //third shelf
                new ShelfSlotEntity
                {
                    SlotOnShelf = 1,
                    Item = items[15],
                    ItemId = items[15].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 2,
                    Item = items[16],
                    ItemId = items[16].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 3,
                    Item = items[17],
                    ItemId = items[17].Id,
                },
            };

            foreach (ShelfSlotEntity sl in msl2)
            {
                context.ShelfSlots.Add(sl);
            }
            context.SaveChanges();
            List<ShelfSlotEntity> msl3 = new List<ShelfSlotEntity>
            {
                //First Shelf
                new ShelfSlotEntity
                {
                    SlotOnShelf = 1,
                    Item = items[18],
                    ItemId = items[18].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 2,
                    Item = items[19],
                    ItemId = items[19].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 3,
                    Item = items[20],
                    ItemId = items[20].Id,
                },
                //Second Shelf
                new ShelfSlotEntity
                {
                    SlotOnShelf = 1,
                    Item = items[21],
                    ItemId = items[21].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 2,
                    Item = items[22],
                    ItemId = items[22].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 3,
                    Item = items[23],
                    ItemId = items[23].Id,
                },
                //third shelf
                new ShelfSlotEntity
                {
                    SlotOnShelf = 1,
                    Item = items[24],
                    ItemId = items[24].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 2,
                    Item = items[25],
                    ItemId = items[25].Id,
                },
                new ShelfSlotEntity
                {
                    SlotOnShelf = 3,
                    Item = items[26],
                    ItemId = items[26].Id,
                },
            };

            foreach (ShelfSlotEntity sl in msl3)
            {
                context.ShelfSlots.Add(sl);
            }

            context.SaveChanges();

            #endregion

            #region Shelves

            var shelfslots = context.ShelfSlots.ToList();

            List<ShelfEntity> milkShelves1 = new List<ShelfEntity>
            {
                new ShelfEntity
                {
                    ShelfNumber = 1,
                    Slots = shelfslots.Take(3).ToList()
                },
                new ShelfEntity
                {
                    ShelfNumber = 2,
                    Slots = shelfslots.Skip(3).Take(3).ToList()
                },
                new ShelfEntity
                {
                    ShelfNumber = 3,
                    Slots = shelfslots.Skip(6).Take(3).ToList()
                },
            };

            foreach(ShelfEntity shelf in milkShelves1)
            {
                context.Shelfs.Add(shelf);
            }
            context.SaveChanges();

            List<ShelfEntity> milkShelves2 = new List<ShelfEntity>
            {
                new ShelfEntity
                {
                    ShelfNumber = 1,
                    Slots = shelfslots.Skip(9).Take(3).ToList()
                },
                new ShelfEntity
                {
                    ShelfNumber = 2,
                    Slots = shelfslots.Skip(12).Take(3).ToList()
                },
                new ShelfEntity
                {
                    ShelfNumber = 3,
                    Slots = shelfslots.Skip(15).Take(3).ToList()
                },
            };

            foreach (ShelfEntity shelf in milkShelves2)
            {
                context.Shelfs.Add(shelf);
            }
            context.SaveChanges();
            List<ShelfEntity> milkShelves3 = new List<ShelfEntity>
            {
                new ShelfEntity
                {
                    ShelfNumber = 1,
                    Slots = shelfslots.Skip(18).Take(3).ToList()
                },
                new ShelfEntity
                {
                    ShelfNumber = 2,
                    Slots = shelfslots.Skip(21).Take(3).ToList()
                },
                new ShelfEntity
                {
                    ShelfNumber = 3,
                    Slots = shelfslots.Skip(24).Take(3).ToList()
                },
            };

            foreach (ShelfEntity shelf in milkShelves3)
            {
                context.Shelfs.Add(shelf);
            }

            context.SaveChanges();

            #endregion

            #region Sections

            var shelves = context.Shelfs.ToList();
            List<SectionEntity> sectionsMilk = new List<SectionEntity>
            {
                new SectionEntity
                {
                    Name = "Milk1",
                    ItemsPerShelf = 3,
                    Shelves = shelves.Take(3).ToList(),
                },
                new SectionEntity
                {
                    Name = "Milk2",
                    ItemsPerShelf = 3,
                    Shelves = shelves.Skip(3).Take(3).ToList(),
                },
                new SectionEntity
                {
                    Name = "Milk3",
                    ItemsPerShelf = 3,
                    Shelves = shelves.Skip(6).Take(3).ToList(),
                },
            };

            foreach (SectionEntity sec in sectionsMilk)
            {
                context.Sections.Add(sec);
            }

            context.SaveChanges();

            #endregion

            #region Aisles

            //List<AisleEntity> aislesCanned = new List<AisleEntity>
            //{
            //    new AisleEntity
            //    {
            //        Name = "1",
            //        SideOfAisle = "Right",
            //        Row = 1,
            //        Column = 1,
            //        Sections = new List<SectionEntity>()
            //    },
            //    new AisleEntity
            //    {
            //        Name = "2",
            //        SideOfAisle = "left",
            //        Row = 1,
            //        Column = 3,
            //        Sections = new List<SectionEntity>()
            //    },
            //    new AisleEntity
            //    {
            //        Name = "2",
            //        SideOfAisle = "right",
            //        Row = 1,
            //        Column = 5,
            //        Sections = new List<SectionEntity>()
            //    },
            //    new AisleEntity
            //    {
            //        Name = "3",
            //        SideOfAisle = "left",
            //        Row = 1,
            //        Column = 7,
            //        Sections = new List<SectionEntity>()
            //    },
            //    new AisleEntity
            //    {
            //        Name = "3",
            //        SideOfAisle = "right",
            //        Row = 1,
            //        Column = 9,
            //        Sections = new List<SectionEntity>()
            //    },
            //     new AisleEntity
            //    {
            //        Name = "4",
            //        SideOfAisle = "left",
            //        Row = 1,
            //        Column = 11,
            //        Sections = new List<SectionEntity>()
            //    },
            //};

            //foreach (AisleEntity a in aislesCanned)
            //{
            //    context.Aisles.Add(a);
            //}

            //context.SaveChanges();

            //List<AisleEntity> coffee_drinks_crackers = new List<AisleEntity>
            //{
            //    new AisleEntity
            //    {
            //        Name = "4",
            //        SideOfAisle = "Right",
            //        Row = 1,
            //        Column = 1,
            //        Sections = new List<SectionEntity>()
            //    },
            //    new AisleEntity
            //    {
            //        Name = "5",
            //        SideOfAisle = "left",
            //        Row = 1,
            //        Column = 3,
            //        Sections = new List<SectionEntity>()
            //    },
            //    new AisleEntity
            //    {
            //        Name = "5",
            //        SideOfAisle = "right",
            //        Row = 1,
            //        Column = 5,
            //        Sections = new List<SectionEntity>()
            //    },
            //    new AisleEntity
            //    {
            //        Name = "6",
            //        SideOfAisle = "left",
            //        Row = 1,
            //        Column = 7,
            //        Sections = new List<SectionEntity>()
            //    },
            //    new AisleEntity
            //    {
            //        Name = "6",
            //        SideOfAisle = "right",
            //        Row = 1,
            //        Column = 9,
            //        Sections = new List<SectionEntity>()
            //    },
            //     new AisleEntity
            //    {
            //        Name = "7",
            //        SideOfAisle = "left",
            //        Row = 1,
            //        Column = 11,
            //        Sections = new List<SectionEntity>()
            //    },
            //};

            //foreach (AisleEntity a in coffee_drinks_crackers)
            //{
            //    context.Aisles.Add(a);
            //}

            List<AisleEntity> milkAisles = new List<AisleEntity>
            {
                new AisleEntity
                {
                    Name = "Backwall",
                    SideOfAisle = "Backwall",
                    Row = 1,
                    Column = 1,
                    Sections = sectionsMilk
                }
            };

            foreach (AisleEntity a in milkAisles)
            {
                context.Aisles.Add(a);
            }

            context.SaveChanges();

            #endregion

            #region Lower Departments

            List<LowerDepartmentEntity> lowerDepartmentsGrocery = new List<LowerDepartmentEntity>
            {
                new LowerDepartmentEntity
                {
                    Name = "Canned/Pasta",
                    Row = 0,
                    Column = 0,
                    AisleCount = 3,
                    AisleStart = 1,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Coffee/Drinks/Crackers",
                    Row = 0,
                    Column = 1,
                    AisleCount = 3,
                    AisleStart = 4,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Healthy Back",
                    Row = 0,
                    Column = 2,
                    AisleCount = 3,
                    AisleStart = 7,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Cooking/Asian/Mexican",
                    Row = 1,
                    Column = 0,
                    AisleCount = 3,
                    AisleStart = 10,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Chips/Cereal",
                    Row = 1,
                    Column = 1,
                    AisleCount = 3,
                    AisleStart = 13,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Healthy front",
                    Row = 1,
                    Column = 2,
                    AisleCount = 3,
                    AisleStart = 16,
                    Aisles = new List<AisleEntity>()
                }
            };

            foreach (LowerDepartmentEntity ld in lowerDepartmentsGrocery)
            {
                context.LowerDepartments.Add(ld);
            }
            context.SaveChanges();

            List<LowerDepartmentEntity> lowerDepartmentsPKB = new List<LowerDepartmentEntity>
            {
                new LowerDepartmentEntity
                {
                    Name = "Paint",
                    Row = 0,
                    Column = 0,
                    AisleCount = 3,
                    AisleStart = 1,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Bathroom",
                    Row = 0,
                    Column = 1,
                    AisleCount = 3,
                    AisleStart = 4,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Kitchen",
                    Row = 0,
                    Column = 2,
                    AisleCount = 3,
                    AisleStart = 7,
                    Aisles = new List<AisleEntity>()
                },
            };

            foreach (LowerDepartmentEntity ld in lowerDepartmentsPKB)
            {
                context.LowerDepartments.Add(ld);
            }
            context.SaveChanges();
            List<LowerDepartmentEntity> dairyLowerDepartment = new List<LowerDepartmentEntity>
            {
                new LowerDepartmentEntity
                {
                    Name = "Milk and Yogurt",
                    Row = 0,
                    Column = 0,
                    AisleCount = 1,
                    Aisles = milkAisles
                },
                new LowerDepartmentEntity
                {
                    Name = "Cheese",
                    Row = 0,
                    Column = 1,
                    AisleCount = 1,
                    Aisles = new List<AisleEntity>()
                },
            };

            foreach (LowerDepartmentEntity ld in dairyLowerDepartment)
            {
                context.LowerDepartments.Add(ld);
            }
            context.SaveChanges();
            List<LowerDepartmentEntity> meatLowerDepartment = new List<LowerDepartmentEntity>
            {
                new LowerDepartmentEntity
                {
                    Name = "Sausage",
                    Row = 0,
                    Column = 0,
                    AisleCount = 1,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Pork and Steak",
                    Row = 0,
                    Column = 1,
                    AisleCount = 1,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Chicken",
                    Row = 0,
                    Column = 2,
                    AisleCount = 1,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Fish",
                    Row = 0,
                    Column = 3,
                    AisleCount = 1,
                    Aisles = new List<AisleEntity>()
                }
            };

            foreach (LowerDepartmentEntity ld in meatLowerDepartment)
            {
                context.LowerDepartments.Add(ld);
            }
            context.SaveChanges();
            List<LowerDepartmentEntity> elec_sports_tools_lowerDepartment = new List<LowerDepartmentEntity>
            {
                new LowerDepartmentEntity
                {
                    Name = "Electronics",
                    Row = 0,
                    Column = 0,
                    AisleCount = 2,
                    AisleStart = 1,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Sports",
                    Row = 0,
                    Column = 1,
                    AisleCount = 3,
                    AisleStart = 3,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Tools",
                    Row = 0,
                    Column = 2,
                    AisleCount = 3,
                    AisleStart = 6,
                    Aisles = new List<AisleEntity>()
                }
            };

            foreach (LowerDepartmentEntity ld in elec_sports_tools_lowerDepartment)
            {
                context.LowerDepartments.Add(ld);
            }
            context.SaveChanges();
            List<LowerDepartmentEntity> house_furniture_lowerDepartment = new List<LowerDepartmentEntity>
            {
                new LowerDepartmentEntity
                {
                    Name = "Furniture",
                    Row = 0,
                    Column = 0,
                    AisleCount = 1,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Lighting and Candles",
                    Row = 0,
                    Column = 1,
                    AisleCount = 3,
                    Aisles = new List<AisleEntity>()
                },
                new LowerDepartmentEntity
                {
                    Name = "Wall Decor",
                    Row = 0,
                    Column = 2,
                    AisleCount = 3,
                    Aisles = new List<AisleEntity>()
                }
            };

            foreach (LowerDepartmentEntity ld in house_furniture_lowerDepartment)
            {
                context.LowerDepartments.Add(ld);
            }

            context.SaveChanges();

            #endregion

            #region Departments

            List<DepartmentEntity> departments = new List<DepartmentEntity>
            {
                new DepartmentEntity
                {
                    Name = "Paint/Kitchen/bathroom",
                    MapLocation = "0,0,1,2",
                    LowerDepartments = lowerDepartmentsPKB,
                    ColumnsDefinitions = "*,*,*",
                    RowDefinitions = "*",
                    StoreMapId = 1
                },
                new DepartmentEntity
                {
                    Name = "Dairy",
                    MapLocation = "0,2,1,2",
                    LowerDepartments = dairyLowerDepartment,
                    ColumnsDefinitions = "*,*",
                    RowDefinitions = "*",
                    StoreMapId = 1
                },
                new DepartmentEntity
                {
                    Name = "Meat",
                    MapLocation = "0,4,1,1",
                    LowerDepartments = meatLowerDepartment,
                    ColumnsDefinitions = "*,*,*,*",
                    RowDefinitions = "*",
                    StoreMapId = 1
                },
                new DepartmentEntity
                {
                    Name = "Elect./Sports/Tools",
                    MapLocation = "1,0,5,1",
                    LowerDepartments = elec_sports_tools_lowerDepartment,
                    ColumnsDefinitions = "*,*,*",
                    RowDefinitions = "*",
                    StoreMapId = 1
                },
                new DepartmentEntity
                {
                    Name = "House/Furniture",
                    MapLocation = "1,1,5,1",
                    LowerDepartments = house_furniture_lowerDepartment,
                    ColumnsDefinitions = "*,*,*",
                    RowDefinitions = "*",
                    StoreMapId = 1
                },
                new DepartmentEntity
                {
                    Name = "Grocery",
                    MapLocation = "1,2,5,2",
                    LowerDepartments = lowerDepartmentsGrocery,
                    ColumnsDefinitions = "*,*,*",
                    RowDefinitions = "*,*",
                    StoreMapId = 1
                },
                new DepartmentEntity
                {
                    Name = "Alcohol",
                    MapLocation = "1,4,2,1",
                    LowerDepartments = new List<LowerDepartmentEntity>(),
                    ColumnsDefinitions = "",
                    RowDefinitions = "",
                    StoreMapId = 1
                },
                new DepartmentEntity
                {
                    Name = "Produce",
                    MapLocation = "3,4,3,1",
                    LowerDepartments = new List<LowerDepartmentEntity>(),
                    ColumnsDefinitions = "",
                    RowDefinitions = "",
                    StoreMapId = 1
                },
                 new DepartmentEntity
                {
                    Name = "Apparel",
                    MapLocation = "0,5,6,1",
                    LowerDepartments = new List<LowerDepartmentEntity>(),
                    ColumnsDefinitions = "",
                    RowDefinitions = "",
                    StoreMapId = 1
                },
                new DepartmentEntity
                {
                    Name = "Cash Registers",
                    MapLocation = "6,2,1,2",
                    LowerDepartments = new List<LowerDepartmentEntity>(),
                    ColumnsDefinitions = "",
                    RowDefinitions = "",
                    StoreMapId = 1
                }
            };

            foreach (DepartmentEntity d in departments)
            {
                context.Departments.Add(d);
            }

            context.SaveChanges();

            #endregion

            #region StoreMap


            StoreMapEntity storeMap = new StoreMapEntity()
            {
                Departments = departments,
                ColumnsDefinitions = "60,70,*,*,*",
                RowDefinitions = "*,*,*,*,*,*,*",
            };

            context.StoreMaps.Add(storeMap);

            context.SaveChanges();
            #endregion
        }

        private static void AddShoppingItems(ShoppingAssistantAPIContext context)
        {
            var shoppingList = context.ShoppingLists.FirstOrDefault(sl => sl.Id == 2);

            var Item1 = context.Items.FirstOrDefault(i => i.Id == 1);
            var Item2 = context.Items.FirstOrDefault(i => i.Id == 2);
            var Item3 = context.Items.FirstOrDefault(i => i.Id == 3);
            var Item4 = context.Items.FirstOrDefault(i => i.Id == 4);
            var Item5 = context.Items.FirstOrDefault(i => i.Id == 5);
            var Item6 = context.Items.FirstOrDefault(i => i.Id == 6);
            var Item7 = context.Items.FirstOrDefault(i => i.Id == 7);

            List<ItemShoppingListLinkEntity> items = new List<ItemShoppingListLinkEntity>
            {
                new ItemShoppingListLinkEntity
                {
                    Item = Item1,
                    ItemId = 1,
                    ShoppingList = shoppingList,
                    ShoppingListId = 2,
                    ItemQuantity = 2
                },
                new ItemShoppingListLinkEntity
                {
                    Item = Item2,
                    ItemId = 2,
                    ShoppingList = shoppingList,
                    ShoppingListId = 2,
                    ItemQuantity = 3
                },
                new ItemShoppingListLinkEntity
                {
                    Item = Item3,
                    ItemId = 3,
                    ShoppingList = shoppingList,
                    ShoppingListId = 2,
                    ItemQuantity = 1
                },
                new ItemShoppingListLinkEntity
                {
                    Item = Item4,
                    ItemId = 4,
                    ShoppingList = shoppingList,
                    ShoppingListId = 2,
                    ItemQuantity = 4
                },

            };

            foreach (ItemShoppingListLinkEntity i in items)
            {
                context.ItemShoppingListLinks.Add(i);
            }
            context.SaveChanges();
        }

        private static void AddItemsToStore(ShoppingAssistantAPIContext context)
        {
            var store = context.Stores.FirstOrDefault(s => s.Id == 1);

            var Item1 = context.Items.FirstOrDefault(i => i.Id == 1);
            var Item2 = context.Items.FirstOrDefault(i => i.Id == 2);
            var Item3 = context.Items.FirstOrDefault(i => i.Id == 3);
            var Item4 = context.Items.FirstOrDefault(i => i.Id == 4);
            var Item5 = context.Items.FirstOrDefault(i => i.Id == 5);
            var Item6 = context.Items.FirstOrDefault(i => i.Id == 6);
            var Item7 = context.Items.FirstOrDefault(i => i.Id == 7);
            var Item8 = context.Items.FirstOrDefault(i => i.Id == 8);
            var Item9 = context.Items.FirstOrDefault(i => i.Id == 9);
            var Item10 = context.Items.FirstOrDefault(i => i.Id == 10);
            var Item11= context.Items.FirstOrDefault(i => i.Id == 11);
            var Item12 = context.Items.FirstOrDefault(i => i.Id == 12);
            var Item13 = context.Items.FirstOrDefault(i => i.Id == 13);
            var Item14 = context.Items.FirstOrDefault(i => i.Id == 14);
            var Item15 = context.Items.FirstOrDefault(i => i.Id == 15);
            var Item16 = context.Items.FirstOrDefault(i => i.Id == 16);
            var Item17 = context.Items.FirstOrDefault(i => i.Id == 17);
            var Item18 = context.Items.FirstOrDefault(i => i.Id == 18);
            var Item19 = context.Items.FirstOrDefault(i => i.Id == 19);
            var Item20 = context.Items.FirstOrDefault(i => i.Id == 20);
            var Item21 = context.Items.FirstOrDefault(i => i.Id == 21);
            var Item22 = context.Items.FirstOrDefault(i => i.Id == 22);
            var Item23 = context.Items.FirstOrDefault(i => i.Id == 23);
            var Item24 = context.Items.FirstOrDefault(i => i.Id == 24);
            var Item25 = context.Items.FirstOrDefault(i => i.Id == 25);
            var Item26 = context.Items.FirstOrDefault(i => i.Id == 26);
            var Item27 = context.Items.FirstOrDefault(i => i.Id == 27);
            var Item28 = context.Items.FirstOrDefault(i => i.Id == 28);
            var Item29 = context.Items.FirstOrDefault(i => i.Id == 29);
            var Item30 = context.Items.FirstOrDefault(i => i.Id == 30);
            var Item31 = context.Items.FirstOrDefault(i => i.Id == 31);
            var Item32 = context.Items.FirstOrDefault(i => i.Id == 32);
            var Item33 = context.Items.FirstOrDefault(i => i.Id == 33);
            var Item34 = context.Items.FirstOrDefault(i => i.Id == 34);

            List<ItemStoreLinkEntity> items = new List<ItemStoreLinkEntity>
            {
                new ItemStoreLinkEntity
                {
                    Item = Item1,
                    ItemId = 1,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item2,
                    ItemId = 2,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item3,
                    ItemId = 3,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item4,
                    ItemId = 4,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item5,
                    ItemId = 5,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item6,
                    ItemId = 6,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item7,
                    ItemId = 7,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item8,
                    ItemId = 8,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item9,
                    ItemId = 9,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item10,
                    ItemId = 10,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item11,
                    ItemId = 11,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item12,
                    ItemId = 12,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item13,
                    ItemId = 13,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item14,
                    ItemId = 14,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item15,
                    ItemId = 15,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item16,
                    ItemId = 16,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item17,
                    ItemId = 17,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item18,
                    ItemId = 18,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item19,
                    ItemId = 19,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item20,
                    ItemId = 20,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item21,
                    ItemId = 21,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item22,
                    ItemId = 22,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item23,
                    ItemId = 7,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item24,
                    ItemId = 7,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item25,
                    ItemId = 7,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item26,
                    ItemId = 7,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item27,
                    ItemId = 7,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item28,
                    ItemId = 7,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item29,
                    ItemId = 7,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item30,
                    ItemId = 7,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item31,
                    ItemId = 7,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item32,
                    ItemId = 7,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item33,
                    ItemId = 7,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
                new ItemStoreLinkEntity
                {
                    Item = Item34,
                    ItemId = 7,
                    Store = store,
                    StoreId = 1,
                    InStock = true,
                    StockAmount = 34,
                    Price = 3,
                    DepartmentId = 2,
                    LowerDepartmentId = 10,
                    AisleId = 1,
                    SectionId = 1,
                    ShelfId = 1,
                    SlotId = 1,
                },
            };

            foreach (ItemStoreLinkEntity i in items)
            {
                context.ItemStoreLinks.Add(i);
            }

            context.SaveChanges();
        }

        private static void AddItems(ShoppingAssistantAPIContext context)
        {
            List<ItemEntity> items = new List<ItemEntity>
            {
                new ItemEntity { Name = "Hood Milk - Fat Free", SpoonacularProductId = 210757  },
                new ItemEntity { Name = "Hood Fat Free Milk", SpoonacularProductId = 210757 },
                new ItemEntity { Name = "Hood Milk - 1% Lowfat", SpoonacularProductId = 210741},
                new ItemEntity { Name = "Hood Milk - Vitamins C & D", SpoonacularProductId = 210808},
                new ItemEntity { Name = "Hood Milk - 2% Reduced Fat", SpoonacularProductId = 210802},
                new ItemEntity { Name = "Hood Vitamins C & D Milk", SpoonacularProductId = 106958},
                new ItemEntity { Name = "Promised Land Milk", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Promised Land Milk", SpoonacularProductId = 210757  },
                new ItemEntity { Name = "Promise Land Dairy Milk Very Berry Strawberry", SpoonacularProductId = 210757 },
                new ItemEntity { Name = "Promised Land Dairy Milk", SpoonacularProductId = 210741},
                new ItemEntity { Name = "Ahold Chocolate milk", SpoonacularProductId = 210808},
                new ItemEntity { Name = "Ahold Chocolate milk", SpoonacularProductId = 210802},
                new ItemEntity { Name = "Turn Chocolate Milk, 0.5 gal", SpoonacularProductId = 106958},
                new ItemEntity { Name = "Kemps 1% Lowfat Milk, 1 gal", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Ahold 1% Low Fat Milk", SpoonacularProductId = 210757  },
                new ItemEntity { Name = "Ahold 1% Low Fat Milk", SpoonacularProductId = 210757 },
                new ItemEntity { Name = "natrel 1% Lowfat Milk", SpoonacularProductId = 210741},
                new ItemEntity { Name = "natrel 2% Milk Fat", SpoonacularProductId = 210808},
                new ItemEntity { Name = "PET Reduced Fat 2% Milk", SpoonacularProductId = 210802},
                new ItemEntity { Name = "PET Reduced Fat 2% Milk", SpoonacularProductId = 106958},
                new ItemEntity { Name = "Lehigh Valley over the Moon Fat Free Milk", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Smart Balance Heart Right Fat Free Milk", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Deans Dairy Pure Fat free Milk", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Mayfield Milk", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Smart Balance Fat Free Milk and Omega-3s", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Smart Balance Lactose-Free Fat Free Milk and Omega-3s", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Horizon Organic Milk 0% Fat-Free", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Lactaid 100% Lactose Free Fat Free Milk", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Shamrock Farms Milk", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Darigold Milk - Fat Free Vitamin A & D", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Horizon Milk", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Lactaid 100% Lactose Free Whole Milk", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Lactaid 100% Lactose Free Reduced Fat Milk", SpoonacularProductId = 27530},
                new ItemEntity { Name = "Lactaid Fat Free Milk 100% Lactose Free", SpoonacularProductId = 27530},
            };

            foreach(ItemEntity item in items)
            {
                context.Add(item);
            }

            context.SaveChanges();
        }

        private static void AddInMemoryData(ShoppingAssistantAPIContext context)
        {

           
        }

        #endregion
    }
}
