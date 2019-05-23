using System;
using System.Collections.Generic;
using System.Linq;

namespace SeniorProject.Api.Models
{
    public class ShoppingList
    {
        private int _totalItems;
        private decimal _totalCost;

        public ShoppingList()
        {
            Items = new List<ShoppingListItem>();
            _totalItems = 0;
            _totalCost = 0;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset TimeOfCreation { get; set; }

        //public Store Store { get; set; }

        public List<ShoppingListItem> Items { get; set; }

        public decimal TotalCost
        {
            get { return _totalCost; }
            set { _totalCost = value; }
        }

        public int TotalItems
        {
            get
            {
                return _totalItems;
            }
            set
            {
                _totalItems = value;
            }
        }

    }
}
