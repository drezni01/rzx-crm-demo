namespace Rzx.Crm.Core.Models
{
    public class Customer : IEntity
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public DateTime Timestamp { get; set; }
        public int EntityId => CustomerId;
    }

    public class CustomerStats
    {
        public int OrderCount { get; set; }
    }
}
