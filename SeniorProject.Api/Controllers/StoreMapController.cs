using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SeniorProject.Api.Models;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Repository;
using unirest_net.http;

namespace SeniorProject.Api.Controllers
{
    [Route("api/[controller]")]
    public class StoreMapController : Controller
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
        private readonly IRepository<DepartmentEntity> _departmentRepository;
        private readonly IRepository<StoreMapEntity> _storeMapRepository;
        private readonly ILowerDepartmentRepository _lowerDepartmentRepository;
        private readonly IAisleRepository _aisleRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IShelfRepository _shelfRepository;
        private readonly IShelfSlotsRepository _shelfSlotsRepository;
        private readonly IUrlHelper _urlHelper;

        public StoreMapController(
            IShoppingUserRepository shoppingUserRepository,
            IStoreUserRepository storeUserRepository,
            IRepository<AdminUserEntity> adminUserRepository,
            IRepository<BaseUserEntity> baseUserRespository,
            IRepository<StoreEntity> storeRepository,
            IRepository<StoreMapEntity> storeMapRepository,
            IRepository<AddressEntity> addressRepository,
            IShoppingListRepository shoppingListRepository,
            IItemShoppingListLinkRepository itemListLinkRepository,
            IItemEntityRepository itemRepository,
            IItemStoreLinkRepository itemStoreLinkRepository,
            ILowerDepartmentRepository lowerDepartmentRepository,
            IRepository<DepartmentEntity> departmentRepository,
            IAisleRepository aisleRepository,
            ISectionRepository sectionRepository,
            IShelfRepository shelfRepository,
            IShelfSlotsRepository shelfSlotsRepository,
            IUrlHelper urlHelper)
        {
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

            _lowerDepartmentRepository = lowerDepartmentRepository;
            _aisleRepository = aisleRepository;
            _sectionRepository = sectionRepository;
            _shelfRepository = shelfRepository;
            _shelfSlotsRepository = shelfSlotsRepository;
            _departmentRepository = departmentRepository;

            _urlHelper = urlHelper;
        }


        [HttpGet("GetLowerDepartments")]
        [Route("{id}")]
        public IActionResult GetLowerDepartments(int id, CancellationToken ct)
        {
            var lowerDepartments = _lowerDepartmentRepository.GetAllEntities(id, ct);
            return Ok(lowerDepartments);
        }

        [HttpGet("GetAisles")]
        [Route("{id}")]
        public IActionResult GetAisles(int id, CancellationToken ct)
        {
            var aisles = _aisleRepository.GetAllEntities(id, ct);
            return Ok(aisles);
        }

        [HttpGet("GetSections")]
        [Route("{id}/{storeId}")]
        public async Task<IActionResult> GetSections(int id, int storeId, CancellationToken ct)
        {
            var sections = _sectionRepository.GetAllEntities(id, ct);

            List<SectionAPP> sectionsAPP = new List<SectionAPP>();

            foreach (SectionEntity sec in sections)
            {
                var shelves = _shelfRepository.GetAllEntities(sec.Id, ct);
                sec.Shelves = shelves;
                SectionAPP section = new SectionAPP()
                {
                    Id = sec.Id,
                    AisleId = sec.AisleId,
                    Name = sec.Name,
                    ItemsPerShelf = sec.ItemsPerShelf,
                    Shelves = new List<ShelfAPP>()
                };

                foreach (ShelfEntity shelf in shelves)
                {
                    var shelfSlots = _shelfSlotsRepository.GetAllEntities(shelf.Id, ct);
                    shelf.Slots = shelfSlots;
                    ShelfAPP shelfapp = new ShelfAPP
                    {
                        Id = shelf.Id,
                        SectionId = shelf.SectionId,
                        ShelfNumber = shelf.ShelfNumber,
                        Slots = new List<ShelfSlotAPP>()
                    };
                    foreach (var slot in shelfSlots)
                    {
                        var item = await _itemRepository.GetEntityAsync(slot.ItemId, ct);
                        var link = await _itemStoreLinkRepository.GetEntityAsync(item.Id, storeId, ct);

                        var department = await _departmentRepository.GetEntityAsync(link.DepartmentId, ct);
                        var lowerDepartment = await _lowerDepartmentRepository.GetEntityAsync(link.LowerDepartmentId, ct);
                        var aisle = await _aisleRepository.GetEntityAsync(link.AisleId, ct);
                        SpoonProductInformation spoonItem = GetSpoonItem(item.SpoonacularProductId);
                        Item itemapp = new Item
                        {
                            Id = item.Id,
                            LinkId = link.Id,
                            Image = "image.jpg",
                            Name = item.Name,
                            Price = link.Price,
                            InStock = link.InStock,
                            StockAmount = link.StockAmount,
                            DepartmentId = link.DepartmentId,
                            LowerDepartmentId = link.LowerDepartmentId,
                            AisleId = link.AisleId,
                            SectionId = link.SectionId,
                            ShelfId = link.ShelfId,
                            SlotId = link.SlotId,
                            LowerDepartment = lowerDepartment.Name,
                            Aisle = aisle.Name,
                            Section = sec.Name,
                            Shelf = shelf.ShelfNumber.ToString(),
                            Slot = slot.SlotOnShelf.ToString(),
                            Department = department.Name, 
                            ProductInformation = spoonItem
                        };

                        if (spoonItem != null)
                        {
                            itemapp.Image = spoonItem.images.First();
                        }

                        ShelfSlotAPP slotApp = new ShelfSlotAPP
                        {
                            Id = slot.Id,
                            ItemId = slot.ItemId,
                            ShelfId = slot.ShelfId,
                            SlotOnShelf = slot.SlotOnShelf,
                            Item = itemapp
                        };

                        shelfapp.Slots.Add(slotApp);
                    }
                    section.Shelves.Add(shelfapp);
                }
                sectionsAPP.Add(section);
            }

            return Ok(sectionsAPP);
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
                string error = "Error: type an items name in the search bar or some sort of tag, such as drinks, candy, chips, etc.";
                return null;
            }
            else
            {

                var serializer = new JsonSerializer();
                var info = JsonConvert.DeserializeObject<SpoonProductInformation>(response.Body);
                string widget = GetNutritionWidget(spoonacularProductId);

                info.nutrition_widget = widget;
                return info;
            }

        }

        [HttpGet("GetShelves")]
        [Route("{id}")]
        public IActionResult GetShelves(int id, CancellationToken ct)
        {
            var shelves = _shelfRepository.GetAllEntities(id, ct);
            return Ok(shelves);
        }

        [HttpGet("GetShelfSlots")]
        [Route("{id}")]
        public IActionResult GetShelfSlots(int id, CancellationToken ct)
        {
            var shelfSlots = _shelfSlotsRepository.GetAllEntities(id, ct);
            return Ok(shelfSlots);
        }

    }
}