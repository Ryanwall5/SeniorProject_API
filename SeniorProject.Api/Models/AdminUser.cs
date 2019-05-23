using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models
{
    public class AdminUser
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
 
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset TimeOfCreation { get; set; }

    }
}
