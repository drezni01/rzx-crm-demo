namespace Rzx.Crm.Core.Models
{
    public class Order : IEntity
    {
        public int OrderId { get; set; }
        public int SalesPersonId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public long Quantity { get; set; }
        public DateTime Timestamp { get; set; }
        public int EntityId => OrderId;
    }
}
