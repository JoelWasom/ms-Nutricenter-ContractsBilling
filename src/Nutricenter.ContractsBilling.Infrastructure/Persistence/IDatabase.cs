using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence
{
    internal interface IDatabase:IDisposable
    {
        void Migrate();
    }
}
