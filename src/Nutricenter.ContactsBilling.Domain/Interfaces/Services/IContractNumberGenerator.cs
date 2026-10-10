using Nutricenter.ContractsBilling.Domain.ValueObjects;
namespace Nutricenter.ContractsBilling.Domain.Interfaces.Services
{
    public interface IContractNumberGenerator
    {
        Task<SequenceNumberGenerator> GenerateAsync(string prefix);
    }
}
