using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Entities
{
    public class LowerDepartmentEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public int AisleCount { get; set; }
        public int AisleStart { get; set; }

        public int DepartmentId { get; set; }
        public List<AisleEntity> Aisles { get; set; } = new List<AisleEntity>();
    }
}
