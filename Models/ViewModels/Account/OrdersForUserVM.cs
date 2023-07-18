namespace OrganicShop2.Models.ViewModels.Account
{
    public class OrdersForUserVm
    {
        public int OrderNumber { get; set; }
        public decimal Total { get; set; }
        public Dictionary<string, int> ProductsAndQty { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
