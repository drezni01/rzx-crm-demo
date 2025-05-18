namespace Rzx.Crm.Core.Models
{
    public interface IEntity
    {
        int EntityId { get; }
        DateTime Timestamp { get; set; }
    }
}
