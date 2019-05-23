using System;
using System.Collections.Generic;

namespace SeniorProject.Api.Models
{
    public class ShoppingUser
    {
        public ShoppingUser()
        {
            ShoppingLists = new List<ShoppingList>();
        }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string Token { get; set; }

        public string Role { get; set; }

        public Store HomeStore { get; set; }

        public List<ShoppingList> ShoppingLists { get; set; }
    }
}
