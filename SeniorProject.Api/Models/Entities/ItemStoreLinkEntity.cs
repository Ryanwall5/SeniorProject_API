using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Entities
{
    public class ItemStoreLinkEntity
    {
        [Key]
        public int Id { get; set; }
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public ItemEntity Item { get; set; }

        public int StoreId { get; set; }

        [ForeignKey("StoreId")]
        public StoreEntity Store { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "In Stock")]
        [Required]
        public bool InStock { get; set; }

        [Display(Name = "Stock Amount")]
        [Required]
        public int StockAmount { get; set; }

        [Required]
        public int SlotId { get; set; }
        [Required]
        public int ShelfId { get; set; }
        [Required]
        public int AisleId { get; set; }
        [Required]
        public int SectionId { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public int LowerDepartmentId { get; set; }
    }
}
