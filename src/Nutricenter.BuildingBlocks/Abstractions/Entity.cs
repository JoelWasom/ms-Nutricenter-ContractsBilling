
namespace Nutricenter.BuildingBlocks.Abstractions
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        private readonly List<DomainEvent> _domainEvents;
        public ICollection<DomainEvent> DomainEvents
        {
            get
            {
                return _domainEvents;
            }
        }
        public Entity(Guid id)
        {
            if (id==Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(id));
            }
            Id = id;
            _domainEvents = [];
        }
        public void AddDomainEvent(DomainEvent domainEvent) { _domainEvents.Add(domainEvent); }
        public void ClearDomainEvents() { _domainEvents.Clear(); }
        protected Entity() { _domainEvents = []; }
    }
}
