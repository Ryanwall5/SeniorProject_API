using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models
{
    public class ItemListLink
    {
        [Required]
        public int ListId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int ItemQuantity { get; set; }
    }
}


