using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SeniorProject.Api.Models;
using SeniorProject.Api.Models.Entities;
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
    public class ItemStoreLinksController : Controller
    {
        private readonly IItemStoreLinkRepository _itemStoreLinkRepository;
        private readonly IItemEntityRepository _itemRepository;
        private readonly IStoreUserRepository _storeUserRepository;


        public ItemStoreLinksController(IItemStoreLinkRepository itemStoreLinkRepository,
            IItemEntityRepository itemRepository,
            IStoreUserRepository storeUserRepository)
        {
            _itemStoreLinkRepository = itemStoreLinkRepository;
            _itemRepository = itemRepository;
            _storeUserRepository = storeUserRepository;
        }


        [HttpGet("GetAllItemsFromStore")]
        [Route("{storeId}")]
        public async Task<IActionResult> GetAllItemsFromStore(int storeId, CancellationToken ct)
        {
            var itemStoreLinks = await _itemStoreLinkRepository.GetAllEntities(storeId, ct);

            List<Item> items = new List<Item>();

            foreach (var link in itemStoreLinks)
            {
                //var itemEntity = await _itemRepository.GetEntityAsync(link.ItemId, ct);
                SpoonProductInformation spoonProduct = GetSpoonItem(link.Item.SpoonacularProductId);

                if (link.Item != null && spoonProduct != null)
                {
                    Item item = new Item
                    {
                        Id = link.ItemId,
                        LinkId = link.Id,
                        Image = spoonProduct.images.First(),
                        Name = link.Item.Name,
                        Price = link.Price,
                        InStock = link.InStock,
                        StockAmount = link.StockAmount,
                        DepartmentId = link.DepartmentId,
                        LowerDepartmentId = link.LowerDepartmentId,
                        AisleId = link.AisleId,
                        SectionId = link.SectionId,
                        ShelfId = link.ShelfId,
                        SlotId = link.SlotId,
                        ProductInformation = spoonProduct
                    };

                    items.Add(item);
                }
            }

            return Ok(items);
        }

        [HttpGet("GetThreeItemsFromStore")]
        [Route("{storeId}")]
        public async Task<IActionResult> GetThreeItemsFromStore(int storeId, CancellationToken ct)
        {
            var itemStoreLinks = await _itemStoreLinkRepository.GetAllEntities(storeId, ct);

            List<Item> items = new List<Item>();

            foreach (var link in itemStoreLinks.Take(3))
            {
                //var itemEntity = await _itemRepository.GetEntityAsync(link.ItemId, ct);
                SpoonProductInformation spoonProduct = GetSpoonItem(link.Item.SpoonacularProductId);

                if (link.Item != null && spoonProduct != null)
                {
                    Item item = new Item
                    {
                        Id = link.ItemId,
                        LinkId = link.Id,
                        Image = spoonProduct.images.First(),
                        Name = link.Item.Name,
                        Price = link.Price,
                        InStock = link.InStock,
                        StockAmount = link.StockAmount,
                        DepartmentId = link.DepartmentId,
                        LowerDepartmentId = link.LowerDepartmentId,
                        AisleId = link.AisleId,
                        SectionId = link.SectionId,
                        ShelfId = link.ShelfId,
                        SlotId = link.SlotId,
                        ProductInformation = spoonProduct
                    };

                    items.Add(item);
                }
            }

            return Ok(items);
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


        [HttpGet("GetItemByLinkIdAsync", Name = nameof(GetItemByLinkIdAsync))]
        [Route("{linkId}")]
        public async Task<IActionResult> GetItemByLinkIdAsync(int linkId, CancellationToken ct)
        {
            var itemStoreLink = await _itemStoreLinkRepository.GetEntityAsync(linkId, ct);

            if (itemStoreLink == null)
            {
                return NoContent();
            }

            var itemEntity = await _itemRepository.GetEntityAsync(itemStoreLink.ItemId, ct);

            if (itemEntity == null)
            {
                return NoContent();
            }

            Item item = new Item
            {
                LinkId = linkId,
                Image = null,
                InStock = itemStoreLink.InStock,
                Name = itemEntity.Name,
                Price = itemStoreLink.Price,
                StockAmount = itemStoreLink.StockAmount,
            };
            ProductInformation productInformation = null;


            if (itemEntity.SpoonacularProductId != 0)
            {
                productInformation = GetSpoonacularProductInformationById(itemEntity.SpoonacularProductId) as ProductInformation;
            }

            return Ok(item);
        }

        [Authorize]
        public async Task<IActionResult> DeleteItemFromStore(int linkId, CancellationToken ct)
        {
            var deletedSuccessfully = await _itemStoreLinkRepository.DeleteEntity(linkId, ct);

            if(deletedSuccessfully)
            {
                return Ok();
            }
            else
            {            
                return BadRequest($"Could Not Delete {linkId}");
            }
        }


        //[Authorize]
        //public async Task<IActionResult> AddItemToStore(int itemId, int storeId, CancellationToken ct)
        //{

        //    ItemStoreLinkEntity itemStoreLinkEntity = new ItemStoreLinkEntity();
            

        //    var deletedSuccessfully = await _itemStoreLinkRepository.AddEntityAsync(linkId, ct);

        //    if (deletedSuccessfully)
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest($"Could Not Delete {linkId}");
        //    }
        //}


        private object GetSpoonacularProductInformationById(int id)
        {
            //HttpResponse<string> response = Unirest.get("https://spoonacular-recipe-food-nutrition-v1.p.rapidapi.com/food/products/{id}")
            //.header("X-RapidAPI-Key", "aJW3b5eOp7mshOTu72ChE00lPeVPp1s0JOcjsntkWNySHxWWSj")
            //.asJson<string>();

            HttpResponse<string> response = Unirest.get($"https://spoonacular-recipe-food-nutrition-v1.p.rapidapi.com/food/products/{id}")
            .header("X-RapidAPI-Key", "aJW3b5eOp7mshOTu72ChE00lPeVPp1s0JOcjsntkWNySHxWWSj")
            .asJson<string>();

            //HttpResponse<string> response = Unirest.get("https://spoonacular-recipe-food-nutrition-v1.p.rapidapi.com/food/ingredients/9266/information?amount=100&unit=gram")
            //.header("X-RapidAPI-Key", "aJW3b5eOp7mshOTu72ChE00lPeVPp1s0JOcjsntkWNySHxWWSj").asJson<string>();


            if (response.Code == 404 || response.Code == 400)
            {
                string error = "Error: Could not find the spoonacular item with that id";
                return null;
            }
            else
            {

                var serializer = new JsonSerializer();
                var productInformation = JsonConvert.DeserializeObject<SpoonProductInformation>(response.Body);

                return productInformation;
            }
        }
    }
}
