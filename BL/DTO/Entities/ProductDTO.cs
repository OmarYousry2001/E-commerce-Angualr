using Microsoft.AspNetCore.Http;
using Shared.DTOs.Base;

namespace BL.DTO.Entities
{
    public class AddProductDTO : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFileCollection Photo { get; set; }

    }
    public class ProductDTO : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public virtual List<PhotoDTO> Photos { get; set; }
        public string CategoryName { get; set; }
        public double rating { get; set; }

    }


}
