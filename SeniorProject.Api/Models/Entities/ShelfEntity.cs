using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Entities
{
    public class ShelfEntity
    {
        [Key]
        public int Id { get; set; }

        public int ShelfNumber { get; set; }

        public int SectionId { get; set; }
        public List<ShelfSlotEntity> Slots { get; set; } = new List<ShelfSlotEntity>();
    }
}
