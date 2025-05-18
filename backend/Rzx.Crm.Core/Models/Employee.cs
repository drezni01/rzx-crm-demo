namespace Rzx.Crm.Core.Models
{
    public class Employee : IEntity
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public DateTime Timestamp { get; set; }
        public int EntityId => EmployeeId;
    }
}
