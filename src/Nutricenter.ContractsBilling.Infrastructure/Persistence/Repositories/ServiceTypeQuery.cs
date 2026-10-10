using Microsoft.EntityFrameworkCore;
using Nutricenter.ContractsBilling.Application.Service.DTOs;
using Nutricenter.ContractsBilling.Domain.Entities;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.Repositories
{
    public class ServiceTypeQuery: IServiceTypeQuery
    {
        private readonly PersistenceDbContext _context;

        public ServiceTypeQuery(PersistenceDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceType>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.ServiceTypes
                .Select(s => new ServiceType
                {
                    Id = s.ServiceTypeId,
                    Codigo = s.Codigo,
                    Nombre = s.Nombre,
                    Descripcion = s.Descripcion,
                    IsActive = s.IsActive
                }).ToListAsync(cancellationToken);
        }
    }
}
