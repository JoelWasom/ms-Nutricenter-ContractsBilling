using Nutricenter.BuildingBlocks.Abstractions;

namespace Nutricenter.ContractsBilling.Domain.Shared
{
    public sealed class AmountValue : ValueObject
    {
        public decimal Value { get; set; }
        public string? Currency { get; set; }

        private AmountValue() { } // EF lo usa

        public AmountValue(decimal value, string? currency = null)
        {
            Value = value;
            Currency = currency;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }

        public AmountValue Multiply(int quantity)
            => new AmountValue(Value * quantity, Currency);
    }

}