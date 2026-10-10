using Nutricenter.BuildingBlocks.Abstractions;
using System.Text.RegularExpressions;

namespace Nutricenter.ContractsBilling.Domain.ValueObjects
{
    public sealed partial class SequenceNumberGenerator : ValueObject
    {
        public string Value { get; set; }

        public SequenceNumberGenerator(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("The contract number is required.", nameof(value));

            if (!MyRegex().IsMatch(value))
                throw new ArgumentException("Invalid contract number format.", nameof(value));

            Value = value;
        }

        public override string ToString() => Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        [GeneratedRegex(@"^[A-Z]{3}-\d{4}-\d{5}$")]
        private static partial Regex MyRegex();
    }
}
