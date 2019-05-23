using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SeniorProject.Api.Models;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Models.FromApp;
using SeniorProject.Api.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using unirest_net.http;

namespace SeniorProject.Api.Controllers
{
    [Route("api/[controller]")]
    public class StoresController : Controller
    {
        private readonly IRepository<StoreEntity> _storeRepository;
        private readonly IRepository<AddressEntity> _addressRepository;
        private readonly IShoppingUserRepository _shoppingUserRepository;
        private readonly IStoreUserRepository _storeUserRepository;
        private readonly IRepository<StoreMapEntity> _storeMapRepository;
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IItemShoppingListLinkRepository _itemListLinkRepository;
        private readonly IItemEntityRepository _itemRepository;
        private readonly IItemStoreLinkRepository _itemStoreLinkRepository;

        private readonly IRepository<DepartmentEntity> _departmentRepository;
        private readonly ILowerDepartmentRepository _lowerDepartmentRepository;
        private readonly IAisleRepository _aisleRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IShelfRepository _shelfRepository;
        private readonly IShelfSlotsRepository _shelfSlotsRepository;


        private readonly IUrlHelper _urlHelper;

        public StoresController(IRepository<StoreEntity> storeRepository,
                                IRepository<AddressEntity> addressRepository,
                                IRepository<StoreMapEntity> storeMapRepository,
                                IStoreUserRepository storeUserRepository,
                                IShoppingUserRepository shoppingUserRepository,
                                IShoppingListRepository shoppingListRepository,
                                IItemShoppingListLinkRepository itemListLinkRepository,
                                IItemEntityRepository itemRepository,
                                IItemStoreLinkRepository itemStoreLinkRepository,
                                ILowerDepartmentRepository lowerDepartmentRepository,
                                IAisleRepository aisleRepository,
                                ISectionRepository sectionRepository,
                                IShelfRepository shelfRepository,
                                IShelfSlotsRepository shelfSlotsRepository,
                                IRepository<DepartmentEntity> departmentRepository,
                                IUrlHelper urlHelper)
        {
            _storeUserRepository = storeUserRepository;
            _storeRepository = storeRepository;
            _addressRepository = addressRepository;
            _urlHelper = urlHelper;

            _storeMapRepository = storeMapRepository;
            _shoppingUserRepository = shoppingUserRepository;
            _shoppingListRepository = shoppingListRepository;
            _shoppingUserRepository = shoppingUserRepository;

            _itemListLinkRepository = itemListLinkRepository;
            _itemRepository = itemRepository;
            _itemStoreLinkRepository = itemStoreLinkRepository;

            _departmentRepository = departmentRepository;
            _lowerDepartmentRepository = lowerDepartmentRepository;
            _aisleRepository = aisleRepository;
            _sectionRepository = sectionRepository;
            _shelfRepository = shelfRepository;
            _shelfSlotsRepository = shelfSlotsRepository;
        }

        // GET: api/Stores
        [HttpGet("GetAllStores")]
        public async Task<IActionResult> GetAllStores(CancellationToken ct)
        {
            var storeEntities = await _storeRepository.GetAllEntities(ct);

            if (storeEntities.Count == 0)
            {
                return NotFound();
            }

            var stores = new List<Store>();

            foreach (var entity in storeEntities)
            {
                var addressEntity = await _addressRepository.GetEntityAsync(entity.AddressId, ct);

                Store mappedStore = Mapper.Map<Store>(entity);

                Address address = Mapper.Map<Address>(addressEntity);

                mappedStore.Address = address;

                StoreMapEntity map = await _storeMapRepository.GetEntityAsync(entity.StoreMapId, ct);

                StoreMap newMap = Mapper.Map<StoreMap>(map);

                mappedStore.StoreMap = newMap;
                stores.Add(mappedStore);
            }

            return Ok(stores);
        }

        [Authorize]
        [HttpPut("ChangeHomeStore")]
        [Route("{id}")]
        public async Task<IActionResult> ChangeHomeStore(int id, CancellationToken ct)
        {
            var currentUser = HttpContext.User;
            var userclaim = currentUser.Claims.First();
            var userId = Guid.Parse(userclaim.Value);
            var shoppingUser = await _shoppingUserRepository.GetEntityAsync(userId, ct);

            if (shoppingUser == null)
            {
                return BadRequest("You are not a shopping user");
            }

            var store = await _storeRepository.GetEntityAsync(id, ct);

            shoppingUser.HomeStoreId = store.Id;

            var updatedUser = await _shoppingUserRepository.UpdateEntity(shoppingUser, ct);

            if (updatedUser.HomeStoreId != store.Id)
            {
                return BadRequest("Error updating users store");
            }

            AddressEntity addressEntity = await _addressRepository.GetEntityAsync(store.AddressId, ct);
            Store mappedStore = Mapper.Map<Store>(store);
            Address address = Mapper.Map<Address>(addressEntity);
            mappedStore.Address = address;

            StoreMapEntity map = await _storeMapRepository.GetEntityAsync(store.StoreMapId, ct);

            StoreMap newMap = Mapper.Map<StoreMap>(map);
            mappedStore.StoreMap = newMap;

            var lists = await _shoppingListRepository.GetAllShoppingListsForUserForACertainStore(shoppingUser.Id, mappedStore.Id, ct);

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

            var updatedUserApp = new ShoppingUser
            {
                FullName = $"{shoppingUser.FirstName} {shoppingUser.LastName}",
                Email = shoppingUser.Email,
                HomeStore = mappedStore,
                ShoppingLists = newlist,
                Role = "Shopping"
            };

            return Ok(updatedUserApp);
        }

        // GET: api/Stores/5
        [HttpGet("{id}", Name = "GetStore")]
        public async Task<IActionResult> GetStore(int id, CancellationToken ct)
        {
            StoreEntity storeEntity = await _storeRepository.GetEntityAsync(id, ct);

            AddressEntity addressEntity = await _addressRepository.GetEntityAsync(storeEntity.AddressId, ct);

            Store mappedStore = Mapper.Map<Store>(storeEntity);

            Address address = Mapper.Map<Address>(addressEntity);

            mappedStore.Address = address;

            StoreMapEntity map = await _storeMapRepository.GetEntityAsync(storeEntity.StoreMapId, ct);
            StoreMap newMap = Mapper.Map<StoreMap>(map);
            mappedStore.StoreMap = newMap;

            return Ok(mappedStore);
        }

        // POST: api/Stores
        [HttpPost("PostStore")]
        public async Task<IActionResult> PostStore([FromBody] APIStore apiStore, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Model state is not valid" });
            }

            AddressEntity address = new AddressEntity
            {
                City = apiStore.City,
                State = apiStore.State,
                Street = apiStore.Street,
                Latitude = apiStore.Latitude,
                Longitude = apiStore.Longitude,
                Zip = apiStore.Zip
            };

            var addressCreated = await _addressRepository.AddEntityAsync(address, ct);
            if (addressCreated)
            {
                StoreEntity store = new StoreEntity
                {
                    Name = apiStore.Name,
                    PhoneNumber = apiStore.PhoneNumber,
                    Website = apiStore.Website,
                    AddressId = address.Id,
                    StoreUserId = apiStore.StoreUserId
                };

                var storeCreated = await _storeRepository.AddEntityAsync(store, ct);
                if (storeCreated)
                {
                    var storeUser = await _storeUserRepository.GetEntityAsync(apiStore.StoreUserId, ct);
                    storeUser.HomeStoreId = store.Id;
                    var updatedStoreUser = _storeUserRepository.UpdateEntity(storeUser, ct);

                    if (updatedStoreUser != null)
                    {
                        return Ok($"store Created = {store}");
                    }
                    return Conflict("Unable to updated StoreUser");
                }
                return Conflict("Store was not created, but address was");
            }

            return Conflict("Store and address were not created");
        }

        // PUT: api/Stores/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            bool storeDeleted = await _storeRepository.DeleteEntity(id, ct);

            if (storeDeleted)
            {
                return Ok();
            }
            else
            {
                return Conflict();
            }
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
