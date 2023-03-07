
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganicShop2.Models.Data
{
    public class PagesDTO
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public int? Sorting { get; set; }
        public bool? HasSidebar { get; set; }
        public string? Image { get; set; }
    }
}
