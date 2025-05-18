namespace Rzx.Crm.Core.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime Timestamp { get; set; }
        public int EntityId => ProductId;
    }
}
