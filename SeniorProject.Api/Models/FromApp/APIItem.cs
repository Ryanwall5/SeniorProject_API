using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.FromApp
{
    public class APIItem
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public bool InStock { get; set; }

        [Required]
        public int StockAmount { get; set; }

        // Aisle in the department that the item is located
        [Required]
        public string Aisle { get; set; }

        // Section in the aisle the item is located
        [Required]
        public string Section { get; set; }

        // Shelf on the aisle in the section that the item is located
        [Required]
        public int Shelf { get; set; }

        // Department where the item is located
        [Required]
        public string Department { get; set; }

        [Required]
        public int SpoonId { get; set; }
    }
}
