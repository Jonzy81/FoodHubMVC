namespace FoodHubMVC.Models
{
    public class MenuItemViewModel
    {
        public int MenuId { get; set; }
        public string MenuType { get; set; }
        public string MenuName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsAwailable { get; set; }
    }
}
