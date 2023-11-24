using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assiment.EF.DTO
{
    public class UpdataProductImage
    {
        public int ProductId { get; set; }
        public IFormFile image { get; set; }

    }
}
