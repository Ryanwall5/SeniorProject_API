using System;

namespace SeniorProject.Api.Models
{
    public class StoreUser
    {
        public string Email { get; set; }

        public string FullName { get; set; }

        public string Token { get; set; }

        public String Role { get; set; }

        public DateTimeOffset TimeOfCreation { get; set; }

        public Store Store { get; set; }
    }
}
