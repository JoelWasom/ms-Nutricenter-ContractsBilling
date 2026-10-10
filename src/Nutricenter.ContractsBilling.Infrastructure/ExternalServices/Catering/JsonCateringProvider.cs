using Microsoft.Extensions.Options;
using Nutricenter.ContractsBilling.Domain.Interfaces.ExternalServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Nutricenter.ContractsBilling.Infrastructure.ExternalServices.Catering
{
    public sealed class JsonCateringProvider: IServiceCatalog
    {
        private readonly string _dataPath;

        public JsonCateringProvider(IOptions<CateringOptions> options)
        {
            _dataPath = options.Value.JsonPath;
        }

        public async Task<ServiceDto?> GetServiceAsync(
            Guid serviceTypeId,
            string serviceId,
            CancellationToken cancellationToken = default)
        {
            var cateringPath = Path.Combine(_dataPath, "DataCatering.json");

            var allServices = new List<ServiceDto>();

            foreach (var path in new[] { cateringPath})
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
    public class CateringOptions
    {
        public string JsonPath { get; set; } = string.Empty;
    }
}
