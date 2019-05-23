using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models
{
    public class LowerDepartment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public int AisleCount { get; set; }
        public int AisleStart { get; set; }

        public int DepartmentId { get; set; }
        //public List<Aisle> Aisles { get; set; } = new List<Aisle>();
    }
}
