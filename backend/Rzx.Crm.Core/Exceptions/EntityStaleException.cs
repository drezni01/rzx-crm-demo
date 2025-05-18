using Rzx.Crm.Core.Models;

namespace Rzx.Crm.Core.Exceptions
{
    public class EntityStaleException : CrmException
    {
        public EntityStaleException(IEntity entity) : base($"entity {entity.GetType().Name}: [{entity.EntityId}] was changed by someone else, please refresh") { }
    }
}
