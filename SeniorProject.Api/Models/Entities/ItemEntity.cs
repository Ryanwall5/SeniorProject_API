using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Entities
{
    
    public class ItemEntity
    {
        public ItemEntity()
        {
            ItemListLinks = new List<ItemShoppingListLinkEntity>();
            ItemStoreLinks = new List<ItemStoreLinkEntity>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SpoonacularProductId { get; set; } = 0;

        [Required]
        public string Name { get; set; }

        public List<ItemShoppingListLinkEntity> ItemListLinks { get; set; }

        public List<ItemStoreLinkEntity> ItemStoreLinks { get; set; }

    }
}
