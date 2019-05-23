using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeniorProject.Api.Models;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Repository;

namespace SeniorProject.Api.Controllers
{
    [Route("api/[controller]")]
    public class ItemShoppingListLinksController : Controller
    {

        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IShoppingUserRepository _shoppingUserRepository;
        private readonly IItemEntityRepository _itemRepository;
        private readonly IItemShoppingListLinkRepository _itemListLinkRepository;
        private readonly IItemStoreLinkRepository _itemStoreLinkRepository;
        public ItemShoppingListLinksController(
            IItemShoppingListLinkRepository itemListLinkRepository,
            IShoppingListRepository shoppingListRepository,
            IItemStoreLinkRepository itemStoreLinkRepository,
            IShoppingUserRepository shoppingUserRepository,
            IItemEntityRepository itemRepository
            )
        {
            _itemListLinkRepository = itemListLinkRepository;
            _shoppingListRepository = shoppingListRepository;
            _shoppingUserRepository = shoppingUserRepository;
            _itemRepository = itemRepository;
            _itemStoreLinkRepository = itemStoreLinkRepository;
        }

        // GET: api/ItemShoppingListLinks
        [HttpGet(Name = nameof(GetAllItemShoppingLists))]
        public IEnumerable<string> GetAllItemShoppingLists()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ItemShoppingListLinks/5
        [HttpGet("{id}", Name = "GetShoppingListItemLink")]
        public string GetItemShoppingListLink(int id)
        {
            return "value";
        }

        [Authorize]
        // POST: api/ItemShoppingListLinks
        [HttpPost("PostItemToList")]
        public async Task<IActionResult> PostItemToList([FromBody]ItemListLink link, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var currentUser = HttpContext.User;
            var userclaim = currentUser.Claims.First();
            var userId = Guid.Parse(userclaim.Value);
            var shoppingUser = await _shoppingUserRepository.GetEntityAsync(userId, ct);

            if(shoppingUser == null)
            {
                return BadRequest("You are not a shopping user");
            }

            ItemShoppingListLinkEntity itemlistlink = new ItemShoppingListLinkEntity
            {
                ItemId = link.ItemId,
                ShoppingListId = link.ListId,
                ItemQuantity = link.ItemQuantity
            };

            // Look up item in the store and make sure it is there.
            var item = await _itemStoreLinkRepository.GetEntityAsync(link.ItemId, shoppingUser.HomeStoreId, ct);

            if(item == null)
            {
                return BadRequest("Item is not in the store");
            }

            var shoppingList = await _shoppingListRepository.GetEntityAsync(link.ListId, ct);

            if(shoppingList.StoreId != shoppingUser.HomeStoreId)
            {
                return BadRequest("List was not created for your current HomeStore");
            }

            var linkAdded = await _itemListLinkRepository.AddNewLink(itemlistlink, ct);
           
            //var linksToShoppingList = await _itemListLinkRepository.GetAllByShoppingListId(link.ListId, ct);

            ShoppingListItem newShoppingListItem = new ShoppingListItem
            {
                LinkId = linkAdded.Id,
            };

            if (linkAdded != null)
            {
                return Ok(newShoppingListItem);
            }

            return BadRequest();
        }

        [Authorize]
        // POST: api/ItemShoppingListLinks
        [HttpGet("GetUsersShoppingItem")]
        [Route("{linkId}")]
        public async Task<IActionResult> GetUsersShoppingItem(int linkId, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var currentUser = HttpContext.User;
            var userclaim = currentUser.Claims.First();
            var userId = Guid.Parse(userclaim.Value);
            var shoppingUser = await _shoppingUserRepository.GetEntityAsync(userId, ct);

            if (shoppingUser == null)
            {
                return BadRequest("You are not a shopping user");
            }

            var link = await _itemListLinkRepository.GetShoppingItem(linkId, ct);

            if(link == null)
            {
                return NotFound("Item Shopping List Link Not Found");
            }

            var item = await _itemRepository.GetEntityAsync(link.ItemId, ct);

            var itemStoreLink = await _itemStoreLinkRepository.GetEntityAsync(link.ItemId, shoppingUser.HomeStoreId, ct);

            var shoppingItem = new ShoppingListItem
            {
                LinkId = link.Id,
                Image = null,
                Name = item.Name,
                Price = itemStoreLink.Price,
                InStock = itemStoreLink.InStock,
                StockAmount = itemStoreLink.StockAmount,
                ItemQuantity = link.ItemQuantity
            };

            return Ok(shoppingItem);
        }


        [Authorize]
        [HttpPut("UpdateLink")]
        [Route("{linkId}/{newQuantity}")]
        public async Task<IActionResult> UpdateLink(int linkId, int newQuantity, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var currentUser = HttpContext.User;
            var userclaim = currentUser.Claims.First();
            var userId = Guid.Parse(userclaim.Value);
            var shoppingUser = await _shoppingUserRepository.GetEntityAsync(userId, ct);

            if (shoppingUser == null)
            {
                return BadRequest("You are not a shopping user");
            }

            var link = await _itemListLinkRepository.GetShoppingItem(linkId, ct);

            if(link == null)
            {
                return BadRequest(new { error = $"Item does not exist in the current store" });
            }

            link.ItemQuantity = newQuantity;
            var updatedLink = await _itemListLinkRepository.UpdateEntity(link, ct);

            ItemListLink newlink = new ItemListLink
            {
                ItemId = updatedLink.ItemId,
                ItemQuantity = updatedLink.ItemQuantity,
                ListId = updatedLink.ShoppingListId
            };

            if (newQuantity == updatedLink.ItemQuantity)
            {
                return Ok(newlink);
            }
            else
            {
                return BadRequest();
            }         
        }

        // PUT: api/ItemShoppingListLinks/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [Authorize]
        [HttpDelete("DeleteLink")]
        [Route("{linkId}")]
        public async Task<IActionResult> DeleteLink(int linkId, CancellationToken ct)
        {
            var currentUser = HttpContext.User;
            var userclaim = currentUser.Claims.First();
            var userId = Guid.Parse(userclaim.Value);
            var shoppingUser = await _shoppingUserRepository.GetEntityAsync(userId, ct);

            if (shoppingUser == null)
            {
                return BadRequest("You are not a shopping user");
            }

            var linkRemoved = await _itemListLinkRepository.DeleteEntity(linkId, ct);

            if(linkRemoved)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
