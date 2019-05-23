using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Entities
{
    public class SectionEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int ItemsPerShelf { get; set; }
        public int AisleId { get; set; }

        public List<ShelfEntity> Shelves { get; set; } = new List<ShelfEntity>();
    }
}
