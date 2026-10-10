using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.ContractsBilling.Domain.Entities;

namespace Nutricenter.ContractsBilling.Domain.Interfaces.Repositories
{
    public interface IServiceTypeQuery
    {
        Task<List<ServiceType>> GetAllAsync(CancellationToken cancellationToken);
    }
}
