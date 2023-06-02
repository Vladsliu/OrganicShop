using System.ComponentModel.DataAnnotations;

namespace OrganicShop2.Models.Data
{
    public class SidebarDTO
    {
        [Key]
        public int id { get; set; }
        public string Body { get; set; }
    }
}
