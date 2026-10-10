
namespace Nutricenter.BuildingBlocks.Abstractions
{
    public interface IRepository<TEntity> where TEntity : AggregateRoot
    {
        Task<TEntity?>GetByIdAsync(Guid id, bool readOnly = false);
        Task CreateAsync(TEntity entity);
    }
}
