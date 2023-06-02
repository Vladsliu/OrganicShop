using OrganicShop2.Models.Data;
using System.ComponentModel.DataAnnotations;

namespace OrganicShop2.Models.ViewModels.Pages
{
    public class PageVM
    {
        public PageVM()
        {

        }
        public PageVM(PagesDTO pagesDTO)
        {
            Id = pagesDTO.Id;
            Title = pagesDTO.Title;
            Slug = pagesDTO.Slug;
            Description = pagesDTO.Description;
            Sorting = pagesDTO.Sorting;
            HasSidebar = pagesDTO.HasSidebar;
            Image = pagesDTO.Image;
        }
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? Title { get; set; }
        public string? Slug { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string? Description { get; set; }
        public int? Sorting { get; set; }
        public bool? HasSidebar { get; set; }

        public string? Image { get; set; }
    }
}
