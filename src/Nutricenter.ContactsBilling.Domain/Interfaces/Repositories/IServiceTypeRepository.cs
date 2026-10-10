using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.ContractsBilling.Domain.Entities;
namespace Nutricenter.ContractsBilling.Domain.Interfaces.Repositories
{
    public interface IServiceTypeRepository: IRepository<ServiceType>
    {
        Task UpdateAsync(ServiceType entity);
        Task DeleteAsync(Guid id);
    }
}
