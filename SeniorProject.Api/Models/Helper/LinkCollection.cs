using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Helper
{
    public abstract class LinkCollection
    {
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
