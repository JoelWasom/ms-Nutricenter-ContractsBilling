using Microsoft.EntityFrameworkCore;
using Nutricenter.ContractsBilling.Domain.Aggregate;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.Repositories
{
    internal class InvoiceRepository(DomainDbContext dbContext): IInvoicetRepository
    {
        private readonly DomainDbContext _dbContext = dbContext;

        public async Task<Invoice?> GetByIdAsync(Guid id, bool readOnly = false)
        {
            if (readOnly)
            {
                return await _dbContext.Invoices
                    .AsNoTracking()
                    .Include(c => c.Services)
                    .FirstOrDefaultAsync(i => i.Id == id);
            }
            else
            {
                return await _dbContext.Invoices
                    .Include(c => c.Services)
                    .FirstOrDefaultAsync(i => i.Id == id);
            }
        }
        public Task CreateAsync(Invoice entity)
        {
            _dbContext.Invoices.AddAsync(entity);
            return Task.CompletedTask; // UoW hará el SaveChanges
        }
        public Task UpdateAsync(Invoice entity)
        {
            _dbContext.Invoices.Update(entity);
            return Task.CompletedTask; // UoW hará el SaveChanges
        }
        public Task DeleteAsync(Guid id)
        {
            var entity = _dbContext.Invoices.Find(id);
            if (entity != null)
            {
                _dbContext.Invoices.Remove(entity);
            }
            return Task.CompletedTask; // UoW hará el SaveChanges
        }
    }
}
