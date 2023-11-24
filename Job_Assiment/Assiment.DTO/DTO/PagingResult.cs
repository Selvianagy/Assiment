using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assiment.EF.DTO
{
    public class PagingResult<T>
    {
        public List<T> Items { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int TotalItems { get; set; }

      
    }
}
