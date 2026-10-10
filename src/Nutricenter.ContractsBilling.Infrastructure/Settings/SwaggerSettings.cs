using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Infrastructure.Settings
{
    public sealed class SwaggerSettings
    {
        public const string SectionName = "Swagger";

        public bool Enabled { get; init; }

        public string Title { get; init; } = string.Empty;

        public string Version { get; init; } = string.Empty;
    }
}
