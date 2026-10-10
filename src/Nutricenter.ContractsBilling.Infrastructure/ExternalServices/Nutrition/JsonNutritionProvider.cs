using Nutricenter.ContractsBilling.Domain.Interfaces.ExternalServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Nutricenter.ContractsBilling.Infrastructure.ExternalServices.Nutrition
{
    public class JsonNutritionProvider: IServiceCatalog
    {
        private readonly string _dataPath;

        public JsonNutritionProvider(string dataPath)
        {
            _dataPath = dataPath ?? throw new ArgumentNullException(nameof(dataPath));
        }

        public async Task<ServiceDto?> GetServiceAsync(
            Guid serviceTypeId,
            string serviceId,
            CancellationToken cancellationToken = default)
        {
            var cateringPath = Path.Combine(_dataPath, "DataNutrition.json");

            var allServices = new List<ServiceDto>();

            foreach (var path in new[] { cateringPath })
            {
                if (!File.Exists(path)) continue;

                var json = await File.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);

                var services = JsonSerializer.Deserialize<List<ServiceDto>>(json)
                               ?? new List<ServiceDto>();

                allServices.AddRange(services);
            }

            return allServices.FirstOrDefault(x =>
                x.ServiceTypeId == serviceTypeId &&
                x.ServiceId == serviceId);
        }
    }
}
