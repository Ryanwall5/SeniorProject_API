using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Entities
{
    public class StoreMapEntity
    {
        [Key]
        public int Id { get; set; }

        // A comma seperated string *,*,60,30,* that will be split and thats how to create row definitions
        public string RowDefinitions { get; set; }
        // A comma seperated string *,*,60,30,* that will be split and thats how to create column definitions
        public string ColumnsDefinitions { get; set; }

        public List<DepartmentEntity> Departments { get; set; } = new List<DepartmentEntity>();
    }
}
