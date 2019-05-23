using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SeniorProject.Api.Models;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Models.FromApp;
using SeniorProject.Api.Repository;
using unirest_net.http;

namespace SeniorProject.Api.Controllers
{
    [Route("api/[controller]")]
    public class ShoppingListsController : Controller
    {

        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IShoppingUserRepository _shoppingUserRepository;
        private readonly IRepository<StoreEntity> _storeRepository;
        private readonly IRepository<AddressEntity> _addressRepository;
        private readonly IItemEntityRepository _itemRepository;
        private readonly IItemStoreLinkRepository _itemStoreLinkRepository;
        private readonly IRepository<StoreMapEntity> _storeMapRepository;
        private readonly ILowerDepartmentRepository _lowerDepartmentRepository;
        private readonly IAisleRepository _aisleRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IShelfRepository _shelfRepository;
        private readonly IShelfSlotsRepository _shelfSlotsRepository;
        private readonly IRepository<DepartmentEntity> _departmentRepository;

        private readonly IItemShoppingListLinkRepository _itemListLinkRepository;
        private readonly IUrlHelper _urlHelper;

        public ShoppingListsController(
             IItemShoppingListLinkRepository itemListLinkRepository,
            IShoppingListRepository shoppingListRepository,
            IShoppingUserRepository shoppingUserRepository,
            IItemEntityRepository itemRepository,
            IRepository<StoreEntity> storeRepository,
            IRepository<AddressEntity> addressRepository,
            IItemStoreLinkRepository itemStoreLinkRepository,
            IRepository<StoreMapEntity> storeMapRepository,
            IRepository<DepartmentEntity> departmentRepository,
            ILowerDepartmentRepository lowerDepartmentRepository,
            IAisleRepository aisleRepository,
            ISectionRepository sectionRepository,
            IShelfRepository shelfRepository,
            IShelfSlotsRepository shelfSlotsRepository,
            IUrlHelper urlHelper)
        {
            _itemListLinkRepository = itemListLinkRepository;
            _storeRepository = storeRepository;
            _addressRepository = addressRepository;
            _shoppingListRepository = shoppingListRepository;
            _shoppingUserRepository = shoppingUserRepository;
            _itemRepository = itemRepository;
            _itemStoreLinkRepository = itemStoreLinkRepository;
            _storeMapRepository = storeMapRepository;
            _urlHelper = urlHelper;

            _departmentRepository = departmentRepository;
            _lowerDepartmentRepository = lowerDepartmentRepository;
            _aisleRepository = aisleRepository;
            _sectionRepository = sectionRepository;
            _shelfRepository = shelfRepository;
            _shelfSlotsRepository = shelfSlotsRepository;
        }

        [Authorize]
        // GET: api/ShoppingLists
        [HttpGet("GetHomeStoreShoppingLists")]
        public async Task<IActionResult> GetHomeStoreShoppingLists(CancellationToken ct)
        {
            var currentUser = HttpContext.User;
            var userclaim = currentUser.Claims.First();
            var userId = Guid.Parse(userclaim.Value);
            var shoppingUser = await _shoppingUserRepository.GetEntityAsync(userId, ct);

            if (shoppingUser == null)
            {
                return BadRequest(new { error = "Not allowed to access shopping lists" });
            }

            var lists = await _shoppingListRepository.GetAllShoppingListsForUserForACertainStore(shoppingUser.Id, shoppingUser.HomeStoreId, ct);

            List<ShoppingList> usersShoppingLists = new List<ShoppingList>();
            foreach (var list in lists)
            {
                ShoppingList sl = new ShoppingList();
                sl.Id = list.Id;
                sl.Name = list.Name;
                sl.TimeOfCreation = list.TimeOfCreation;

                var itemListLinks = await _itemListLinkRepository.GetAllByShoppingListId(list.Id, ct);
                //TODO: Look up item in shoppinglist item link repository
                foreach (var itemlistlink in itemListLinks)
                {
                    ShoppingListItem item = await GetFullItemInfo(itemlistlink, list.StoreId, ct);
                    sl.Items.Add(item);
                }

                sl.TotalCost = sl.Items.Select(i => i.Price * i.ItemQuantity).ToList().Sum();
                sl.TotalItems = sl.Items.Select(i => i.ItemQuantity).ToList().Sum();

                usersShoppingLists.Add(sl);
            }

            return Ok(usersShoppingLists);
        }

        // GET: api/ShoppingLists/5
        [HttpGet("{id}", Name = "GetShoppingList")]
        public string GetShoppingList(int id)
        {
            return "value";
        }

        // POST: api/ShoppingLists
        [Authorize]
        [HttpPost("PostShoppingList")]
        public async Task<IActionResult> PostShoppingList([FromBody]APIShoppingList shoppingList, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Model state is not valid" });
            }

            var currentUser = HttpContext.User;
            var userclaim = currentUser.Claims.First();
            var userId = Guid.Parse(userclaim.Value);
            var shoppingUser = await _shoppingUserRepository.GetEntityAsync(userId, ct);

            if (shoppingUser != null)
            {
                ShoppingListEntity list = new ShoppingListEntity
                {
                    Name = shoppingList.Name,
                    ShoppingUserId = shoppingUser.Id,
                    StoreId = shoppingUser.HomeStoreId,
                    TimeOfCreation = DateTimeOffset.UtcNow
                };

                var listAdded = await _shoppingListRepository.AddShoppingList(list, ct);

                if (listAdded.Item1)
                {
                    shoppingUser.ShoppingLists.Add(list);
                    var user = await _shoppingUserRepository.UpdateEntity(shoppingUser, ct);

                    return Ok(new ShoppingList { Id = list.Id, Items = new List<ShoppingListItem>(), Name = list.Name, TimeOfCreation = list.TimeOfCreation, TotalCost = 0, TotalItems = 0 });
                }
                else
                {
                    return BadRequest(new { error = listAdded.Item2 });
                }
            }

            return BadRequest(new { error = "Could not find shopping user" });
        }

        [Authorize]
        // PUT: api/ShoppingLists/5
        [HttpPut("UpdateList")]
        [Route("{listId}")]
        public async Task<IActionResult> UpdateList(int listId, [FromBody]APIShoppingList shoppingListNewName, CancellationToken ct)
        {
            var currentUser = HttpContext.User;
            var userclaim = currentUser.Claims.First();
            var userId = Guid.Parse(userclaim.Value);
            var shoppingUser = await _shoppingUserRepository.GetEntityAsync(userId, ct);

            if(shoppingUser == null)
            {
                return BadRequest(new { error = "Could not find shopping user" });
            }

            var shoppingList = await _shoppingListRepository.GetEntityAsync(listId, ct);

            if(shoppingList == null)
            {
                return BadRequest(new { error = $"Error updating shopping list with new name {shoppingListNewName.Name}, Could not find shopping list" });
            }

            shoppingList.Name = shoppingListNewName.Name;

            var updatedList = await _shoppingListRepository.UpdateEntity(shoppingList, ct);

            if(updatedList == null)
            {
                return BadRequest(new { error = $"Error updating shopping list with new name {shoppingListNewName.Name}" });
            }

            return Ok($"Shopping list updated with new name \"{updatedList.Name}\"");
        }

        [Authorize]
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var currentUser = HttpContext.User;
            var userclaim = currentUser.Claims.First();
            var userId = Guid.Parse(userclaim.Value);
            var shoppingUser = await _shoppingUserRepository.GetEntityAsync(userId, ct);

            if (shoppingUser == null)
            {
                return BadRequest(new { error = "Not allowed to access shopping lists" });
            }

            var shoppingListDeleted = await _shoppingListRepository.DeleteEntity(id, ct);

            if (shoppingListDeleted)
            {
                return Ok();
            }

            return BadRequest();
        }



        //TODO: convert all this stuff and get the image from spoonacular
        private async Task<ShoppingListItem> ConvertToShoppingListItem(ItemShoppingListLinkEntity itemShoppingListLink, int storeId, CancellationToken ct)
        {

            /*
             *     public class ShoppingListItem
                   {
                    public int Id { get; set; }

                    public string Image { get; set; }

                    public string Name { get; set; }

                    public decimal Price { get; set; }

                    public bool InStock { get; set; }

                    public int StockAmount { get; set; }

                    public string Aisle { get; set; }

                    public string Section { get; set; }

                    public int QuantityBought { get; set; }
                   }
             * 
             */

            ItemEntity itemEntity = await _itemRepository.GetEntityAsync(itemShoppingListLink.ItemId, ct);
            ItemStoreLinkEntity itemStoreLink = await _itemStoreLinkRepository.GetEntityAsync(itemEntity.Id, storeId, ct);

            ShoppingListItem shoppingListItem = new ShoppingListItem
            {
                LinkId = itemShoppingListLink.Id,
                Name = itemEntity.Name,
                Price = itemStoreLink.Price,
                InStock = itemStoreLink.InStock,
                StockAmount = itemStoreLink.StockAmount,
                ItemQuantity = itemShoppingListLink.ItemQuantity
            };

            return shoppingListItem;
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
    }
}
