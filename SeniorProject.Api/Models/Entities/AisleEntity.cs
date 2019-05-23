using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Entities
{
    public class AisleEntity
    {
        [Key]
        public int Id { get; set; }

        // 1, 2, 3, 4, 5
        public string Name { get; set; }

        // Left or Right
        public string SideOfAisle { get; set; }

        public int Row { get; set; }
        public int Column { get; set; }

        public int LowerDepartmenttId { get; set; }
        public List<SectionEntity> Sections { get; set; } = new List<SectionEntity>();
    }
}
