using SeniorProject.Api.Models;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Models.FromApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Infrastructure
{
    public class Converters
    {
        public ShoppingListItem ConvertToShoppingListItem(ItemShoppingListLinkEntity itemShoppingListLink)
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

                    public int? QuantityBought { get; set; }
                   }
             * 
             */

            ShoppingListItem shoppingListItem = new ShoppingListItem();



            return shoppingListItem;
        }
    }
}
