using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Helper
{
    public class SpoonProduct
    {
        public int id { get; set; }

        public string title { get; set; }

        public string image { get; set; }

        public string imageType { get; set; }
    }

    public class ProductArray
    {
        public SpoonProduct[] products { get; set; }
    }
}
