namespace SmartSupplyChain.Core.Entities
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }

        // Navigation
        public Company Company { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    }
}
