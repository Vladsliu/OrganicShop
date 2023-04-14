using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrganicShop2.Models.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganicShop2.Models.ViewModels
{
    public class CreateProductDTOViewModel
    {
        private ProductDTO dto;

        public CreateProductDTOViewModel()
        {
        }

        public CreateProductDTOViewModel(ProductDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Slug = row.Slug;
            Description = row.Description;
            Price = row.Price;
            CategoryName = row.CategoryName;
            CategoryId = row.CategoryId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string? CategoryName { get; set; }
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
 
        public IEnumerable<SelectListItem>? Categories { get; set; }

    }
}
