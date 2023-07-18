using System.ComponentModel.DataAnnotations;

namespace OrganicShop2.Models.ViewModels
{
    public class CreatePagesDTOViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public int? Sorting { get; set; }
        public bool? HasSidebar { get; set; }
        public IFormFile? Image { get; set; }
    }
}
