using System.ComponentModel.DataAnnotations;

namespace OrganicShop2.Models.Data
{
    public class CategoryDTO
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public int? Sorting { get; set; }

    }
}
