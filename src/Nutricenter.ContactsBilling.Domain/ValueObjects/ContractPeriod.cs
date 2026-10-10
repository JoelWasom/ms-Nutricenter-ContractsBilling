using Nutricenter.BuildingBlocks.Abstractions;

namespace Nutricenter.ContractsBilling.Domain.ValueObjects
{
    public sealed class ContractPeriod : ValueObject
    {
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public ContractPeriod(DateOnly startDate, DateOnly endDate)
        {
            if (endDate <= startDate)
                throw new ArgumentException("End date must be greater than start date");

            StartDate = startDate;
            EndDate = endDate;
        }

        public int TotalDays => EndDate.DayNumber - StartDate.DayNumber;

        public bool IsActive(DateOnly today) => today >= StartDate && today <= EndDate;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartDate;
            yield return EndDate;
        }
    }
}
