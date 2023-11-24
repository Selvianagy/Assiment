using Microsoft.AspNetCore.Http;

namespace Assiment.DTO
{
    public class ProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quentity { get; set; }
        public IFormFile image { get; set; }
    }
}
