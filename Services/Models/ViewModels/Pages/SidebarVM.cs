using OrganicShop2.Models.Data;

namespace OrganicShop2.Models.ViewModels.Pages
{
    public class SidebarVM
    {
        public SidebarVM()
        {

        }
        public SidebarVM(SidebarDTO row)
        {
            Id = row.id;
            Body = row.Body;
        }
        public int Id { get; set; }
        public string Body { get; set; }
    }
}
