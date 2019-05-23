using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // row,column,rowspan,columnspan Comma Seperated
        public string MapLocation { get; set; }
        // for the department above it would have 3 sets
        // A comma seperated string *,*,60,30,* that will be split and thats how to create row definitions
        public string RowDefinitions { get; set; }
        // A comma seperated string *,*,60,30,* that will be split and thats how to create column definitions
        public string ColumnsDefinitions { get; set; }

        public int StoreMapId { get; set; }

        public List<LowerDepartment> LowerDepartments { get; set; } = new List<LowerDepartment>();
    }
}
