using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SeniorProject.Api.Models;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Models.FromApp;
using SeniorProject.Api.Models.Helper;
using SeniorProject.Api.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using unirest_net.http;

namespace SeniorProject.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IShoppingUserRepository _shoppingUserRepository;
        private readonly IStoreUserRepository _storeUserRepository;
        private readonly IRepository<AdminUserEntity> _adminUserRepository;
        private readonly IRepository<BaseUserEntity> _baseUserRepository;
        private readonly IRepository<StoreEntity> _storeRepository;
        private readonly IRepository<AddressEntity> _addressRepository;
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IItemShoppingListLinkRepository _itemListLinkRepository;
        private readonly IItemEntityRepository _itemRepository;
        private readonly IItemStoreLinkRepository _itemStoreLinkRepository;

        private readonly IRepository<StoreMapEntity> _storeMapRepository;
        private readonly IRepository<DepartmentEntity> _departmentRepository;
        private readonly ILowerDepartmentRepository _lowerDepartmentRepository;
        private readonly IAisleRepository _aisleRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IShelfRepository _shelfRepository;
        private readonly IShelfSlotsRepository _shelfSlotsRepository;

        private readonly IUrlHelper _urlHelper;
        //private UserManager<StoreUserEntity> _storeUserManager;
        //private SignInManager<StoreUserEntity> _storeSignInManager;
        //private UserManager<ShoppingUserEntity> _shoppingUserManager;
        //private SignInManager<ShoppingUserEntity> _shoppingSignInManager;
        private UserManager<BaseUserEntity> _baseUserManager;
        private SignInManager<BaseUserEntity> _baseSignInManager;
        private PasswordHasher<ShoppingUserEntity> _passwordHasher = new PasswordHasher<ShoppingUserEntity>();

        public UsersController(
            //UserManager<StoreUserEntity> storeUserManager,
            //SignInManager<StoreUserEntity> storeSignInManager,
            //UserManager<ShoppingUserEntity> shoppingUserManager,
            //SignInManager<ShoppingUserEntity> shoppingSignInManager,
            UserManager<BaseUserEntity> baseUserManager,
            SignInManager<BaseUserEntity> baseSignInManager,
            IShoppingUserRepository shoppingUserRepository,
            IStoreUserRepository storeUserRepository,
            IRepository<AdminUserEntity> adminUserRepository,
            IRepository<BaseUserEntity> baseUserRespository,
            IRepository<StoreEntity> storeRepository,
            IRepository<StoreMapEntity> storeMapRepository,
            IRepository<DepartmentEntity> departmentRepository,
            IRepository<AddressEntity> addressRepository,
            IShoppingListRepository shoppingListRepository,
            IItemShoppingListLinkRepository itemListLinkRepository,
            IItemEntityRepository itemRepository,
            IItemStoreLinkRepository itemStoreLinkRepository,
            ILowerDepartmentRepository lowerDepartmentRepository,
            IAisleRepository aisleRepository,
            ISectionRepository sectionRepository,
            IShelfRepository shelfRepository,
            IShelfSlotsRepository shelfSlotsRepository,
            IUrlHelper urlHelper)
        {
            _baseUserManager = baseUserManager;
            _baseSignInManager = baseSignInManager;
            //_storeUserManager = storeUserManager;
            //_storeSignInManager = storeSignInManager;
            //_shoppingUserManager = shoppingUserManager;
            //_shoppingSignInManager = shoppingSignInManager;
            _shoppingUserRepository = shoppingUserRepository;
            _storeUserRepository = storeUserRepository;
            _adminUserRepository = adminUserRepository;
            _baseUserRepository = baseUserRespository;
            _storeRepository = storeRepository;
            _addressRepository = addressRepository;
            _shoppingListRepository = shoppingListRepository;
            _itemListLinkRepository = itemListLinkRepository;
            _itemRepository = itemRepository;
            _itemStoreLinkRepository = itemStoreLinkRepository;
            _storeMapRepository = storeMapRepository;

            _departmentRepository = departmentRepository;
            _lowerDepartmentRepository = lowerDepartmentRepository;
            _aisleRepository = aisleRepository;
            _sectionRepository = sectionRepository;
            _shelfRepository = shelfRepository;
            _shelfSlotsRepository = shelfSlotsRepository;

            _urlHelper = urlHelper;
        }

        /// <summary>
        /// Only user by the Admin to get all of the store users
        /// TODO: need to authoirze the user and make sure it is an admin user
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [Authorize]
        // GET: api/Users
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var users = await _baseUserRepository.GetAllEntities(ct);

            List<BaseUser> baseUsers = new List<BaseUser>();

            foreach (var user in users)
            {
                baseUsers.Add(new BaseUser
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    TimeOfCreation = user.TimeOfCreation
                });
            }

            return Ok(baseUsers);
        }


        /// <summary>
        /// Only user by the Admin to get all of the store users
        /// TODO: need to authoirze the user and make sure it is an admin user
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [Authorize]
        // GET: api/Users
        [HttpGet("GetAllStoreUsers")]
        public async Task<IActionResult> GetAllStoreUsers(CancellationToken ct)
        {
            var users = await _storeUserRepository.GetAllEntities(ct);

            List<StoreUser> storeUsers = new List<StoreUser>();

            foreach (var user in users)
            {
                if (user.HomeStoreId == 0)
                {
                    return Conflict(new { error = $"StoreEntity's Homestore is not set {user.Email}" });
                }
                var storeUser = new StoreUser
                {
                    Token = CreateToken(user),
                    FullName = $"{user.FirstName}, {user.LastName}",
                    Email = user.Email,
                    TimeOfCreation = user.TimeOfCreation
                };

                if (user.HomeStoreId != 0)
                {
                    StoreEntity storeEntity = await _storeRepository.GetEntityAsync(user.HomeStoreId, ct);
                    AddressEntity addressEntity = await _addressRepository.GetEntityAsync(storeEntity.AddressId, ct);
                    Store mappedStore = Mapper.Map<Store>(storeEntity);
                    Address address = Mapper.Map<Address>(addressEntity);
                    mappedStore.Address = address;
                    storeUser.Store = mappedStore;
                }
                else
                {
                    storeUser.Store = null;
                }

                storeUsers.Add(storeUser);
            }

            return Ok(storeUsers);
        }

        // POST: api/Users/PostShoppingUser
        [HttpPost("RegisterStoreUser")]
        public async Task<IActionResult> RegisterStoreUser([FromBody]APIStoreUser user, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Model state is not valid" });
            }

            var storeUser = new StoreUserEntity
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Email,
                Email = user.Email,
            };

            var result = await _baseUserManager.CreateAsync(storeUser, user.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _baseUserManager.AddToRoleAsync(storeUser, "Store");

            //await _baseSignInManager.SignInAsync(storeUser, isPersistent: false);

            await _baseUserManager.UpdateAsync(storeUser);

            return Ok(CreateToken(storeUser));
        }

        // POST: api/Users/PostShoppingUser
        [HttpPost("RegisterShoppingUser")]
        public async Task<IActionResult> RegisterShoppingUser([FromBody]APIShoppingUser user, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Model state is not valid" });
            }

            StoreEntity store = await _storeRepository.GetEntityAsync(user.HomeStoreId, ct);
            var shoppingUser = new ShoppingUserEntity
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                HomeStoreId = user.HomeStoreId
            };

            //var result = await _shoppingUserManager.CreateAsync(shoppingUser, user.Password);
            //await _shoppingUserManager.AddToRoleAsync(shoppingUser, "Shopping");

            var result = await _baseUserManager.CreateAsync(shoppingUser, user.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            result = await _baseUserManager.AddToRoleAsync(shoppingUser, "Shopping");
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            //await _baseSignInManager.SignInAsync(shoppingUser, isPersistent: false);

            result = await _baseUserManager.UpdateAsync(shoppingUser);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(CreateToken(shoppingUser));
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody]LoginUser user, CancellationToken ct)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Model state is not valid" });
            }

            var result = await _baseSignInManager.PasswordSignInAsync(user.Email, user.Password, false, false);

            if (!result.Succeeded)
            {
                return BadRequest(new { error = $"Unable to sign in user {user.Email}" });
            }

            var userEntity = await _baseUserManager.FindByEmailAsync(user.Email);

            var rolenames = await _baseUserManager.GetRolesAsync(userEntity) as List<string>;

            if (rolenames[0] == "Shopping")
            {
                try
                {
                    var shoppingUserEntity = userEntity as ShoppingUserEntity;
                    StoreEntity storeEntity = await _storeRepository.GetEntityAsync(shoppingUserEntity.HomeStoreId, ct);
                    AddressEntity addressEntity = await _addressRepository.GetEntityAsync(storeEntity.AddressId, ct);
                    Store mappedStore = Mapper.Map<Store>(storeEntity);
                    Address address = Mapper.Map<Address>(addressEntity);
                    mappedStore.Address = address;

                    StoreMapEntity map = await _storeMapRepository.GetEntityAsync(storeEntity.StoreMapId, ct);


                    StoreMap newMap = Mapper.Map<StoreMap>(map);

                    mappedStore.StoreMap = newMap;

                    var lists = await _shoppingListRepository.GetAllShoppingListsForUserForACertainStore(shoppingUserEntity.Id, mappedStore.Id, ct);

                    List<ShoppingList> newlist = new List<ShoppingList>();

                    //sle = ShoppingListEntity
                    foreach (var sle in lists)
                    {
                        ShoppingList sl = new ShoppingList
                        {
                            Name = sle.Name,
                            //Store = mappedStore,
                            TimeOfCreation = sle.TimeOfCreation,
                            Id = sle.Id
                        };

                        var itemListLinkEntities = await _itemListLinkRepository.GetAllByShoppingListId(sle.Id, ct);
                        foreach (var itemlistlink in itemListLinkEntities)
                        {
                            ShoppingListItem item = await GetFullItemInfo(itemlistlink, sle.StoreId, ct);
                            sl.Items.Add(item);
                        }

                        sl.TotalCost = sl.Items.Select(i => i.Price * i.ItemQuantity).ToList().Sum();
                        sl.TotalItems = sl.Items.Select(i => i.ItemQuantity).ToList().Sum();

                        newlist.Add(sl);
                    }

                    var shoppingUser = new ShoppingUser
                    {
                        Token = CreateToken(shoppingUserEntity),
                        FullName = $"{shoppingUserEntity.FirstName} {shoppingUserEntity.LastName}",
                        Email = shoppingUserEntity.Email,
                        HomeStore = mappedStore,
                        ShoppingLists = newlist,
                        Role = "Shopping"
                    };

                    var response = new ObjectResult(shoppingUser)
                    {
                        StatusCode = (int)HttpStatusCode.OK
                    };

                    Request.HttpContext.Response.Headers.Add("authorization", shoppingUser.Token);


                    return response;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
            else if (rolenames[0] == "Store")
            {

                var storeUserEntity = userEntity as StoreUserEntity;
                var storeUser = new StoreUser
                {
                    Token = CreateToken(storeUserEntity),
                    FullName = $"{storeUserEntity.FirstName}, {storeUserEntity.LastName}",
                    Email = storeUserEntity.Email,
                    Role = "Store"
                };

                if (storeUserEntity.HomeStoreId != 0)
                {
                    StoreEntity storeEntity = await _storeRepository.GetEntityAsync(storeUserEntity.HomeStoreId, ct);
                    AddressEntity addressEntity = await _addressRepository.GetEntityAsync(storeEntity.AddressId, ct);
                    Store mappedStore = Mapper.Map<Store>(storeEntity);
                    Address address = Mapper.Map<Address>(addressEntity);
                    mappedStore.Address = address;
                    storeUser.Store = mappedStore;
                }
                else
                {
                    storeUser.Store = null;
                }

                var response = new ObjectResult(storeUser)
                {
                    StatusCode = (int)HttpStatusCode.OK
                };

                Request.HttpContext.Response.Headers.Add("authorization", storeUser.Token);


                return response;
            }
            else if (rolenames[0] == "Admin")
            {

            }

            return BadRequest(new { error = $"Could not find user with role {rolenames[0]}" });
        }

        [HttpPost("LoginShoppingUser")]
        public async Task<IActionResult> LoginShoppingUser([FromBody]LoginUser user, CancellationToken ct)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(new { error = "Model state is not valid" });
            //}

            //var result = await _baseSignInManager.PasswordSignInAsync(user.Email, user.Password, false, false);

            //if (!result.Succeeded)
            //{
            //    return BadRequest(new { error = $"Unable to sign in user {user.Email}" });
            //}

            var shoppingUserEntity = await _baseUserManager.FindByEmailAsync(user.Email) as ShoppingUserEntity;

            //// Get Home Store Information
            StoreEntity storeEntity = await _storeRepository.GetEntityAsync(shoppingUserEntity.HomeStoreId, ct);
            AddressEntity addressEntity = await _addressRepository.GetEntityAsync(storeEntity.AddressId, ct);
            Store mappedStore = Mapper.Map<Store>(storeEntity);
            Address address = Mapper.Map<Address>(addressEntity);
            mappedStore.Address = address;

            var shoppingUser = new ShoppingUser
            {
                Token = CreateToken(shoppingUserEntity),
                FullName = $"{shoppingUserEntity.FirstName}, {shoppingUserEntity.LastName}",
                Email = shoppingUserEntity.Email,
                HomeStore = mappedStore
            };

            return Ok(shoppingUser);
        }


        [HttpPost("LoginStoreUser")]
        public async Task<IActionResult> LoginStoreUser([FromBody]LoginUser user, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Model state is not valid" });
            }

            var result = await _baseSignInManager.PasswordSignInAsync(user.Email, user.Password, false, false);

            if (!result.Succeeded)
            {
                return BadRequest(new { error = $"Unable to sign in user {user.Email}" });
            }

            var storeUserEntity = await _baseUserManager.FindByEmailAsync(user.Email) as StoreUserEntity;
            // Get Home Store Information

            var storeUser = new StoreUser
            {
                Token = CreateToken(storeUserEntity),
                FullName = $"{storeUserEntity.FirstName}, {storeUserEntity.LastName}",
                Email = storeUserEntity.Email
            };

            if (storeUserEntity.HomeStoreId != 0)
            {
                StoreEntity storeEntity = await _storeRepository.GetEntityAsync(storeUserEntity.HomeStoreId, ct);
                AddressEntity addressEntity = await _addressRepository.GetEntityAsync(storeEntity.AddressId, ct);
                Store mappedStore = Mapper.Map<Store>(storeEntity);
                Address address = Mapper.Map<Address>(addressEntity);
                mappedStore.Address = address;
                storeUser.Store = mappedStore;
            }
            else
            {
                storeUser.Store = null;
            }

            return Ok(storeUser);
        }

        // GET: api/Users/GetShoppingUser
        [HttpGet("GetShoppingUser")]
        public async Task<IActionResult> GetShoppingUser(string email, CancellationToken ct)
        {
            var user = await _shoppingUserRepository.GetEntityAsync(email, ct);

            if (user == null)
            {
                return NotFound(new { error = "User Not Found with that email, Create a new user or Try Again!" });
            }

            // Get Home Store Information
            StoreEntity storeEntity = await _storeRepository.GetEntityAsync(user.HomeStoreId, ct);
            AddressEntity addressEntity = await _addressRepository.GetEntityAsync(storeEntity.AddressId, ct);
            Store mappedStore = Mapper.Map<Store>(storeEntity);
            Address address = Mapper.Map<Address>(addressEntity);
            mappedStore.Address = address;

            // Get Shopping Lists
            //var usersShoppingLists = await _shoppingListRepository.GetAllShoppingListsForUserForACertainStore(user.Id, user.HomeStoreId, ct);
            List<ShoppingList> list = new List<ShoppingList>();

            // sle = ShoppingListEntity
            foreach (var sle in user.ShoppingLists)
            {
                ShoppingList sl = new ShoppingList
                {
                    Name = sle.Name,
                    //Store = mappedStore,
                    TimeOfCreation = sle.TimeOfCreation,
                    Id = sle.Id
                };
                //var itemListLinkEntities = await _itemListLinkRepository.GetAllByShoppingListId(sle.Id, ct);
                foreach (var itemlistlink in sle.ListItemLinks)
                {
                    ShoppingListItem item = await GetFullItemInfo(itemlistlink, sle.StoreId, ct);
                    sl.Items.Add(item);
                }
                list.Add(sl);
            }

            ShoppingUser theShoppingUser = Mapper.Map<ShoppingUser>(user);
            theShoppingUser.HomeStore = mappedStore;
            theShoppingUser.ShoppingLists = list;
            // Grab all of the items for each of the shopping lists
            return Ok(theShoppingUser);
        }



        // POST: api/Users/PostShoppingUser
        //[HttpPost("PostShoppingUser")]
        //public async Task<IActionResult> PostShoppingUser([FromBody]APIShoppingUser user, CancellationToken ct)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new { error = "Model state is not valid" });
        //    }
        //    //TODO: Remove this only use when using postman because postman cant hash passwords
        //    PasswordHasher<ShoppingUserEntity> passwordHasher = new PasswordHasher<ShoppingUserEntity>();
        //    ShoppingUserEntity entity = new ShoppingUserEntity();
        //    string password = passwordHasher.HashPassword(entity, user.hashedPassword);
        //    user.hashedPassword = password;

        //    StoreEntity store = await _storeRepository.GetEntityAsync(user.homeStoreId, ct);
        //    // returns a tuple of bool, string  or (true if created, no message) and (false if not created and an error message)
        //    Tuple<bool, string> userCreated = await _shoppingUserRepository.AddEntityAsync(user);

        //    if (userCreated.Item1)
        //    {
        //        return Ok($"user Created = {user.email}");
        //    }
        //    else
        //    {
        //        return BadRequest(new { error = userCreated.Item2 });
        //    }
        //}

        // POST: api/Users/PostShoppingUser
        //[HttpPost("PostStoreUser")]
        //public async Task<IActionResult> PostStoreUser([FromBody]APIStoreUser user, CancellationToken ct)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new { error = "Model state is not valid" });
        //    }
        //    //TODO: Remove this only use when using postman because postman cant hash passwords
        //    PasswordHasher<ShoppingUserEntity> passwordHasher = new PasswordHasher<ShoppingUserEntity>();
        //    ShoppingUserEntity entity = new ShoppingUserEntity();
        //    string password = passwordHasher.HashPassword(entity, user.hashedPassword);
        //    user.hashedPassword = password;

        //    // returns a tuple of bool, string  or (true if created, no message) and (false if not created and an error message)
        //    Tuple<bool, string> userCreated = await _storeUserRepository.AddEntityAsync(user,ct);

        //    if (userCreated.Item1)
        //    {
        //        return Ok($"user Created = {user.email}");
        //    }
        //    else
        //    {
        //        return BadRequest(new { error = userCreated.Item2 });
        //    }
        //}

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var userDeleted = await _shoppingUserRepository.DeleteEntity(id, ct);

            if (userDeleted)
            {
                return Ok();
            }

            return NotFound("User not found to delete");
        }


        private async Task<ShoppingListItem> GetFullItemInfo(ItemShoppingListLinkEntity itemlistlink, int storeId, CancellationToken ct)
        {
            var itemEntity = await _itemRepository.GetEntityAsync(itemlistlink.ItemId, ct);
            SpoonProductInformation spoonItem = GetSpoonItem(itemEntity.SpoonacularProductId);
            var itemStoreLink = await _itemStoreLinkRepository.GetEntityAsync(itemlistlink.ItemId, storeId, ct);

            var department = await _departmentRepository.GetEntityAsync(itemStoreLink.DepartmentId, ct);
            var lowerDepartment = await _lowerDepartmentRepository.GetEntityAsync(itemStoreLink.LowerDepartmentId, ct);
            var aisle = await _aisleRepository.GetEntityAsync(itemStoreLink.AisleId, ct);
            var section = await _sectionRepository.GetEntityAsync(itemStoreLink.SectionId, ct);
            var shelf = await _shelfRepository.GetEntityAsync(itemStoreLink.ShelfId, ct);
            var slot = await _shelfSlotsRepository.GetEntityAsync(itemStoreLink.SlotId, ct);

            ShoppingListItem item = new ShoppingListItem
            {
                LinkId = itemlistlink.Id,
                Image = "image.jpg",
                Name = itemEntity.Name,
                Price = itemStoreLink.Price,
                InStock = itemStoreLink.InStock,
                StockAmount = itemStoreLink.StockAmount,
                ItemQuantity = itemlistlink.ItemQuantity,
                DepartmentId = itemStoreLink.DepartmentId,
                Department = department.Name,
                LowerDepartmentId = itemStoreLink.LowerDepartmentId,
                LowerDepartment = lowerDepartment.Name,
                AisleId = itemStoreLink.AisleId,
                Aisle = aisle.Name,
                SectionId = itemStoreLink.SectionId,
                Section = section.Name,
                ShelfId = itemStoreLink.ShelfId,
                Shelf = shelf.ShelfNumber.ToString(),
                SlotId = itemStoreLink.SlotId,
                Slot = slot.SlotOnShelf.ToString()
            };

            if (spoonItem != null)
            {
                item.Image = spoonItem.images.First();
            }

            return item;
        }

        private SpoonProductInformation GetSpoonItem(int spoonacularProductId)
        {
            string url = $"https://spoonacular-recipe-food-nutrition-v1.p.rapidapi.com/food/products/{spoonacularProductId}";
            HttpResponse<string> response = Unirest.get(url)
            .header("X-RapidAPI-Key", "aJW3b5eOp7mshOTu72ChE00lPeVPp1s0JOcjsntkWNySHxWWSj")
            .asJson<string>();

            if (response.Code == 404 || response.Code == 400)
            {
                string error = "Error: type an items name in the search bar or some sort of tag, such as drinks, candy, chips, etc.";
                return null;
            }
            else
            {

                var serializer = new JsonSerializer();
                var info = JsonConvert.DeserializeObject<SpoonProductInformation>(response.Body);

                return info;
            }

        }

        private string CreateToken(ShoppingUserEntity shoppingUser)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, shoppingUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, "Shopping"),
            };

            var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Shopping Assistant Secret Code"));
            var signingCredentials = new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }


        private string CreateToken(StoreUserEntity storeUser)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, storeUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, "Store"),
            };

            var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Shopping Assistant Secret Code"));
            var signingCredentials = new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
