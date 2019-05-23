using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Entities
{
    public class ShoppingListEntity
    {
        public ShoppingListEntity()
        {
            ListItemLinks = new List<ItemShoppingListLinkEntity>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset TimeOfCreation { get; set; }
   
        public Guid ShoppingUserId { get; set; }

        public int StoreId { get; set; }

        public List<ItemShoppingListLinkEntity> ListItemLinks { get; set; }
    }
}
