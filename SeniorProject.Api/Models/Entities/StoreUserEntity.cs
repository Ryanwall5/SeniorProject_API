using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Entities
{
    public class StoreUserEntity : BaseUserEntity
    {
        public int HomeStoreId { get; set; }
    }
}
