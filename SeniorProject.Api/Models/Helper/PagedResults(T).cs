using System.Collections.Generic;

namespace SeniorProject.Api.Models.Helper
{
    public class PagedResults<T>
    {
        public IEnumerable<T> Entities { get; set; }
        public int Size { get; set; }
    }
}
