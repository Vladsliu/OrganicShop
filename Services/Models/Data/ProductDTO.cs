using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganicShop2.Models.Data
{
    public class ProductDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string? CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryDTO? Categories { get; set; }

    }
}
