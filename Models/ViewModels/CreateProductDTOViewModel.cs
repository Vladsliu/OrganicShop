using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrganicShop2.Models.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganicShop2.Models.ViewModels
{
    public class CreateProductDTOViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string? CategoryName { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? Image { get; set; }
        [ForeignKey("CategoryId")]
        public virtual CategoryDTO? Categories { get; set; }
    }
}
