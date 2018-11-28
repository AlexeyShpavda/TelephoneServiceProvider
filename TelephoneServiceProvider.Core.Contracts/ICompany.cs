using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace TelephoneServiceProvider.Core.Contracts
{
    public interface ICompany
    {
        string Name { get; }

        ICollection<Client> Clients { get; }

        ICollection<Contract> Contracts { get; }

        Billing Billing { get; }

        BaseStation BaseStation { get; }

    }
}