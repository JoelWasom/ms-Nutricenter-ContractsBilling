using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Infrastructure.Settings
{
    public sealed class DatabaseSettings
    {
        public const string SectionName = "Database";

        public string Provider { get; init; } = string.Empty;

        public string ConnectionString { get; init; } = string.Empty;

        public bool EnableSensitiveDataLogging { get; init; }

        public bool EnableDetailedErrors { get; init; }
    }
}
