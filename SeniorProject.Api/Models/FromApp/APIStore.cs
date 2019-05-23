using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.FromApp
{
    public class APIStore
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Website { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public Guid StoreUserId { get; set; }

    }
}
