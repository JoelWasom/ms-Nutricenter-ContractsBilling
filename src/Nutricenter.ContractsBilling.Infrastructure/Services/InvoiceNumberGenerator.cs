using Microsoft.EntityFrameworkCore;
using Nutricenter.ContractsBilling.Domain.Interfaces.Services;
using Nutricenter.ContractsBilling.Domain.ValueObjects;
using Nutricenter.ContractsBilling.Infrastructure.Persistence;

namespace Nutricenter.ContractsBilling.Infrastructure.Services
{
    public sealed class InvoiceNumberGenerator(PersistenceDbContext context): IInvoiceNumberGenerator
    {
        private readonly PersistenceDbContext _context = context;

        public async Task<SequenceNumberGenerator> GenerateAsync(string prefix)
        {
            var year = DateTime.UtcNow.Year;

            var lastNumber = await _context.Invoices
                .Where(c => c.NumeroFactura.StartsWith($"{prefix}-{year}-"))
                .OrderByDescending(c => c.NumeroFactura)
                .Select(c => c.NumeroFactura)
                .FirstOrDefaultAsync();

            var nextSequence = 1;

            if (!string.IsNullOrWhiteSpace(lastNumber))
            {
                var sequence = int.Parse(lastNumber.Split('-')[2]);
                nextSequence = sequence + 1;
            }

            var contractNumber = $"{prefix}-{year}-{nextSequence:D5}";

            return new SequenceNumberGenerator(contractNumber);
        }
    }
}
