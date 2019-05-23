using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Entities
{
    public class ShoppingUserEntity : BaseUserEntity
    {
        public ShoppingUserEntity()
        {
            ShoppingLists = new List<ShoppingListEntity>();
        }
        
        public int HomeStoreId { get; set; }

        public List<ShoppingListEntity> ShoppingLists { get; set; }

    }
}
