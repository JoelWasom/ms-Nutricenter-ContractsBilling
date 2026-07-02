using Nutricenter.BuildingBlocks.Abstractions;

namespace Nutricenter.ContractsBilling.Domain.Shared
{
    public sealed class AmountValue(decimal value, string? currency = null) : ValueObject
    {
        public decimal Value { get; } = value;
        public string? Currency { get; } = currency;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }
    }
}