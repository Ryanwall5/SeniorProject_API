using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models
{
    public class ShelfSlotAPP
    {
        public int Id { get; set; }

        public int SlotOnShelf { get; set; }

        public int ShelfId { get; set; }

        public int ItemId { get; set; }

        public Item Item { get; set; }
    }

    public class SectionAPP
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ItemsPerShelf { get; set; }

        public int AisleId { get; set; }

        public List<ShelfAPP> Shelves { get; set; } = new List<ShelfAPP>();
    }

    public class ShelfAPP
    {
        public int Id { get; set; }

        public int ShelfNumber { get; set; }

        public int SectionId { get; set; }

        public List<ShelfSlotAPP> Slots { get; set; } = new List<ShelfSlotAPP>();
    }
}
