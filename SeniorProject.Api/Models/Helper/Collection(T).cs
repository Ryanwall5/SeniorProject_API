using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models.Helper
{
    public class Collection<T> : LinkCollection where T : LinkCollection
    {
        public IEnumerable<T> Value { get; set; }
    }
}
