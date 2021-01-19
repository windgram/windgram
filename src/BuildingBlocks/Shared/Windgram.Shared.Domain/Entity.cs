namespace Windgram.Shared.Domain
{

    public abstract class Entity : IEntity { }
    public abstract class Entity<TKey> : Entity, IEntity<TKey>
    {
        public virtual TKey Id { get; set; }
    }
}
