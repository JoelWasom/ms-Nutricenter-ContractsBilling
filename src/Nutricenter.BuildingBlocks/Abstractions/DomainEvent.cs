
namespace Nutricenter.BuildingBlocks.Abstractions
{
    public abstract record DomainEvent
    {
        public Guid Id { get; set; }
        public DateTime OccuredOn { get; set; }
        public DomainEvent()
        {
            Id= Guid.NewGuid();
            OccuredOn = DateTime.Now;
        }
    }
}
