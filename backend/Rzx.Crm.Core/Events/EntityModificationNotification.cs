
using MediatR;

namespace Rzx.Crm.Core.Events
{
    public enum EntityModificationTypeEnum
    {
        ADD,
        UPDATE,
        DELETE
    }

    public class EntityModificationNotification<T> : INotification
    {
        public T Entity { get; }
        public EntityModificationTypeEnum ModificationType { get; }

        public EntityModificationNotification(T entity, EntityModificationTypeEnum modificationType)
        {
            Entity = entity;
            ModificationType = modificationType;
        }
    }
}
