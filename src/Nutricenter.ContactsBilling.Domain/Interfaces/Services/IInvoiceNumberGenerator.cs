using Nutricenter.ContractsBilling.Domain.ValueObjects;
namespace Nutricenter.ContractsBilling.Domain.Interfaces.Services
{
    public interface IInvoiceNumberGenerator
    {
        Task<SequenceNumberGenerator> GenerateAsync(string prefix);
    }
}
