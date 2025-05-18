namespace Rzx.Crm.Core.Configuration
{
    public class SeedDataConfig
    {
        public SeedTypeEnum SeedType { get; set; }
        public int EmployeesCount { get; set; }
        public int ProductsCount { get; set; } 
        public int CustomersCount { get; set; }
        public int OrdersCount { get; set; }
    }

    public enum SeedTypeEnum
    {
        ALWAYS,
        WHEN_EMPTY
    }
}
