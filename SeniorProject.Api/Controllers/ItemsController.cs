using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SeniorProject.Api.Models;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Models.Helper;
using SeniorProject.Api.Repository;
using unirest_net.http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeniorProject.Api.Models.FromApp;
using Microsoft.AspNetCore.Authorization;

namespace SeniorProject.Api.Controllers
{

    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private readonly IItemEntityRepository _itemRepository;
        private readonly IItemStoreLinkRepository _itemStoreLinkRepository;
        private readonly IStoreUserRepository _storeUserRepository;

        private readonly ILowerDepartmentRepository _lowerDepartmentRepository;
        private readonly IAisleRepository _aisleRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IShelfRepository _shelfRepository;
        private readonly IShelfSlotsRepository _shelfSlotsRepository;
        private readonly IRepository<DepartmentEntity> _departmentRepository;

        private readonly IRepository<StoreEntity> _storeRepository;
        private readonly IUrlHelper _urlHelper;
        private ProductArray _products;

        public ItemsController(IItemEntityRepository itemRepository,
                    IStoreUserRepository storeUserRepository,
                    IItemStoreLinkRepository itemStoreLinkRepository,
                    IRepository<StoreEntity> storeRepository,
                                IRepository<DepartmentEntity> departmentRepository,
            ILowerDepartmentRepository lowerDepartmentRepository,
            IAisleRepository aisleRepository,
            ISectionRepository sectionRepository,
            IShelfRepository shelfRepository,
            IShelfSlotsRepository shelfSlotsRepository,
                    IUrlHelper urlHelper)
        {
            _storeRepository = storeRepository;
            _itemRepository = itemRepository;
            _itemStoreLinkRepository = itemStoreLinkRepository;
            _urlHelper = urlHelper;
            _storeUserRepository = storeUserRepository;
            _departmentRepository = departmentRepository;
            _lowerDepartmentRepository = lowerDepartmentRepository;
            _aisleRepository = aisleRepository;
            _sectionRepository = sectionRepository;
            _shelfRepository = shelfRepository;
            _shelfSlotsRepository = shelfSlotsRepository;
        }

        // GET: api/<controller>
        [HttpGet(Name = nameof(GetAllItemsAsync))]
        public async Task<IActionResult> GetAllItemsAsync(CancellationToken ct)
        {
            var foundItems = GetSpoonacularItems("milk");
            if (foundItems != null)
            {
                return Ok(foundItems.products);
            }

            var itemEntities = await _itemRepository.GetEntitiesAsync(ct);

            var items = new Item[itemEntities.Size];

            return Ok(items);
        }


        //public int Id { get; set; }

        //public string Image { get; set; }

        //public string Name { get; set; }

        //public decimal Price { get; set; }

        //public bool InStock { get; set; }

        //public int StockAmount { get; set; }

        //public string Aisle { get; set; }

        //public string Section { get; set; }
        // GET /<controller>/5
        [HttpGet("GetItemByIdAsync", Name = nameof(GetItemByIdAsync))]
        [Route("{itemId}")]
        public async Task<IActionResult> GetItemByIdAsync(int itemId, CancellationToken ct)
        {
            var itemEntity = await _itemRepository.GetEntityAsync(itemId, ct);

            if (itemEntity == null)
            {
                return NoContent();
            }

            var itemStoreLink = itemEntity.ItemStoreLinks.FirstOrDefault(itemLink => itemLink.ItemId == itemEntity.Id);

            if (itemStoreLink == null)
            {
                return NoContent();
            }

            Item item = new Item
            {
                LinkId = itemStoreLink.Id,
                Image = null,
                InStock = itemStoreLink.InStock,
                Name = itemEntity.Name,
                Price = itemStoreLink.Price,
                StockAmount = itemStoreLink.StockAmount,
            };


            return Ok(item);
        }

        [Authorize]
        // POST api/<controller>
        [HttpPost("PostItemToStore")]
        public async Task<IActionResult> PostItemToStore([FromBody]APIItem item, CancellationToken ct)
        {
            //TODO: Check to make sure the store is trying to add the item
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Model state is not valid" });
            }

            var currentUser = HttpContext.User;
            var userclaim = currentUser.Claims.First();
            var userId = Guid.Parse(userclaim.Value);
            var storeUser = await _storeUserRepository.GetEntityAsync(userId, ct);

            if (storeUser == null)
            {
                return BadRequest(new { error = "Not a store, can't add items unless you are a store." });
            }

            if (storeUser.HomeStoreId == 0)
            {
                return BadRequest(new { error = "You are a store, but haven't created your homestore yet." });
            }

            ItemEntity itemEntity = new ItemEntity
            {
                Name = item.Name,
                SpoonacularProductId = item.SpoonId
            };

            ItemStoreLinkEntity linkEntity = new ItemStoreLinkEntity
            {
                InStock = item.InStock,
                StockAmount = item.StockAmount,
                StoreId = storeUser.HomeStoreId,
                Price = item.Price
            };

            bool itemAdded = await _itemRepository.AddEntityAsync(itemEntity, ct);

            if (itemAdded)
            {
                linkEntity.ItemId = itemEntity.Id;
                itemAdded = await _itemStoreLinkRepository.AddEntityAsync(linkEntity, ct);

                if (itemAdded)
                {
                    return Ok();
                }

                return BadRequest();
            }

            return BadRequest();
        }

        // PUT api/<controller>/5
        [HttpPut("{itemId}", Name = nameof(PutItem))]
        public IActionResult PutItem(int itemId, [FromBody]string value, CancellationToken ct)
        {
            return Ok();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{itemId}", Name = nameof(DeleteItem))]
        public IActionResult DeleteItem(int itemId)
        {
            return Ok();
        }

        [HttpGet("{listName}", Name = nameof(GetShoppingListItems))]
        public IActionResult GetShoppingListItems(string listName, CancellationToken ct)
        {
            //var foundItems = GetSpoonacularItems(searchItem);

            //if (foundItems != null)
            //{
            //    var itemStoreLinkEntities = await _itemStoreLinkRepository.GetAllEntities(storeId, ct);

            //    if (itemStoreLinkEntities.Count > 0)
            //    {
            //        var items = await GetItemsToSendToAPI(itemStoreLinkEntities, foundItems, ct);

            //        if (items.Count > 0)
            //        {
            //            return Ok(items);
            //        }

            //        return NotFound(new { error = $"No items found with search {searchItem}, from our database that matched spoonacular" });
            //    }

            //    return NotFound(new { error = $"No items found with search {searchItem}, store contains no items" });
            //}

            //return BadRequest(new { error = $"No items found with search {searchItem}, from spoonacular" });
            return Ok();
        }


        [HttpGet("SearchItems")]
        [Route("{searchItem}/{storeId}")]
        public async Task<IActionResult> SearchItems(string searchItem, int storeId, CancellationToken ct)
        {
            var itemStoreLinkEntities = await _itemStoreLinkRepository.GetAllEntities(storeId, ct);

            List<Item> itemsFound = new List<Item>();
            foreach (var link in itemStoreLinkEntities)
            {
                var itemEntity = await _itemRepository.GetEntityAsync(link.ItemId, ct);
                
                if(itemEntity.Name.ToLower().Contains(searchItem.ToLower()))
                {
                    var item = await GetFullItemInfo(link, itemEntity, ct);
                    itemsFound.Add(item);
                }                
            }

            return Ok(itemsFound);

            //if (foundItems != null)
            //{


            //    if (itemStoreLinkEntities.Count > 0)
            //    {
            //        var items = GetItemsToSendToAPI(itemStoreLinkEntities, foundItems, ct);

            //        if (items.Count > 0)
            //        {
            //            return Ok(items);
            //        }

            //        return NotFound(new { error = $"No items found with search {searchItem}, from our database that matched spoonacular" });
            //    }

            //    return NotFound(new { error = $"No items found with search {searchItem}, store contains no items" });
            //}

            //return BadRequest(new { error = $"No items found with search {searchItem}, from spoonacular" });
        }


        private async Task<Item> GetFullItemInfo(ItemStoreLinkEntity itemStoreLink, ItemEntity itemEntity, CancellationToken ct)
        {

            SpoonProductInformation spoonItem = GetSpoonItem(itemEntity.SpoonacularProductId);

            var department = await _departmentRepository.GetEntityAsync(itemStoreLink.DepartmentId, ct);
            var lowerDepartment = await _lowerDepartmentRepository.GetEntityAsync(itemStoreLink.LowerDepartmentId, ct);
            var aisle = await _aisleRepository.GetEntityAsync(itemStoreLink.AisleId, ct);
            var section = await _sectionRepository.GetEntityAsync(itemStoreLink.SectionId, ct);
            var shelf = await _shelfRepository.GetEntityAsync(itemStoreLink.ShelfId, ct);
            var slot = await _shelfSlotsRepository.GetEntityAsync(itemStoreLink.SlotId, ct);

            Item item = new Item
            {
                Id = itemEntity.Id,
                LinkId = itemStoreLink.Id,
                Image = "image.jpg",
                Name = itemEntity.Name,
                Price = itemStoreLink.Price,
                InStock = itemStoreLink.InStock,
                StockAmount = itemStoreLink.StockAmount,
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
                Slot = slot.SlotOnShelf.ToString(),
                ProductInformation = spoonItem
            };

            if (spoonItem != null)
            {
                item.Image = spoonItem.images.First();
            }

            return item;
        }

        private string GetNutritionWidget(int spoonacularProductId)
        {
            HttpResponse<string> response = Unirest.get($"https://spoonacular-recipe-food-nutrition-v1.p.rapidapi.com/food/products/{spoonacularProductId}/nutritionWidget")
           .header("X-RapidAPI-Host", "spoonacular-recipe-food-nutrition-v1.p.rapidapi.com")
           .header("X-RapidAPI-Key", "aJW3b5eOp7mshOTu72ChE00lPeVPp1s0JOcjsntkWNySHxWWSj")
           .header("Accept", "text/html")
           .asJson<string>();

            if (response.Code == 404 || response.Code == 400)
            {
                return null;
            }
            else
            {
                return response.Body;
            }
        }

        private SpoonProductInformation GetSpoonItem(int spoonacularProductId)
        {

            string url = $"https://spoonacular-recipe-food-nutrition-v1.p.rapidapi.com/food/products/{spoonacularProductId}";
            HttpResponse<string> response = Unirest.get(url)
            .header("X-RapidAPI-Key", "aJW3b5eOp7mshOTu72ChE00lPeVPp1s0JOcjsntkWNySHxWWSj")
            .asJson<string>();

            if (response.Code == 404 || response.Code == 400)
            {
                string error = "Error: could not get product information";
                return null;
            }
            else
            {

                var serializer = new JsonSerializer();
                var info = JsonConvert.DeserializeObject<SpoonProductInformation>(response.Body);

                string widget = GetNutritionWidget(spoonacularProductId);


                //widget = widget.Replace('\\', '"');

                //string fixed_widget = "";
                //foreach(char c in widget)
                //{
                //    if(c == '\\')
                //    {
                //        fixed_widget += '"';
                //    }
                //    fixed_widget += c;
                //}


                info.nutrition_widget = widget;
                return info;
            }

        }

        private List<Item> GetItemsToSendToAPI(List<ItemStoreLinkEntity> itemStoreLinkEntities, ProductArray foundItems, CancellationToken ct)
        {
            List<Item> items = new List<Item>();

            foreach (var link in itemStoreLinkEntities)
            {
                //var itemEntity = await _itemRepository.GetEntityAsync(link.ItemId, ct);
                SpoonProduct spoonProduct = foundItems.products.FirstOrDefault(prod => prod.id == link.Item.SpoonacularProductId);

                if (link.Item != null && spoonProduct != null)
                {
                    Item item = new Item
                    {
                        Id = link.ItemId,
                        LinkId = link.Id,
                        Image = spoonProduct.image,
                        Name = link.Item.Name,
                        Price = link.Price,
                        InStock = link.InStock,
                        StockAmount = link.StockAmount,
                    };

                    items.Add(item);
                }
            }

            return items;
        }


        private ProductArray GetSpoonacularItems(string itemSearch)
        {
            string url = "https://spoonacular-recipe-food-nutrition-v1.p.rapidapi.com/food/products/search?offset=0&number=10&query=" + itemSearch;

            //HttpResponse<string> jsonResponse = Unirest.get(url + itemSearch + "&offset=0&number=20")
            //   .header("X-Mashape-Key", "Jf6NGHfG8fmshjk2T0KyfCFWsr6Dp1La9KzjsnFcmo6L9KcAIh")
            //   .header("X-Mashape-Host", "spoonacular-recipe-food-nutrition-v1.p.mashape.com")

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
                _products = JsonConvert.DeserializeObject<ProductArray>(response.Body);

                return _products;
            }
        }


    }
}
