using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Entities
{
    public class ItemShoppingListLinkEntity
    {
        [Key]
        public int Id { get; set; }

        public int ShoppingListId { get; set; }

        [ForeignKey("ShoppingListId")]
        public ShoppingListEntity ShoppingList { get; set; }

        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public ItemEntity Item { get; set; }

        public int ItemQuantity { get; set; }
    }
}
