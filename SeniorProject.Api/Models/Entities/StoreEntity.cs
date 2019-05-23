using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Entities
{
    public class StoreEntity
    {
        public StoreEntity()
        {
            ItemStoreLinks = new List<ItemStoreLinkEntity>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Website")]
        public string Website { get; set; }

        public int AddressId { get; set; }

        public Guid StoreUserId { get; set; }

        public int StoreMapId { get; set; }

        public List<ItemStoreLinkEntity> ItemStoreLinks { get; set; }
    }
}
