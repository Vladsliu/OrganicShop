using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrganicShop2.Models.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganicShop2.Models.ViewModels.Shop
{
    public class ProductVM
    {
        public ProductVM() { }
        public ProductVM(ProductDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Slug= row.Slug;
            Description= row.Description;
            Price= row.Price;
            CategoryName = row.CategoryName;
            CategoryId = row.CategoryId;
            Image = row.Image;
         
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required]
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public string? Image { get; set; }
       

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}
