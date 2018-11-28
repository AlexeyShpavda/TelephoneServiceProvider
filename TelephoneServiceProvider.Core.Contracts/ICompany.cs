﻿using System.Collections.Generic;
using TelephoneServiceProvider.BillingSystem.Contracts;
using TelephoneServiceProvider.Core.Contracts.Clients;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange;

namespace TelephoneServiceProvider.Core.Contracts
{
    public interface ICompany
    {
        string Name { get; }

        ICollection<IClient> Clients { get; }

        ICollection<IContract> Contracts { get; }

        IBilling Billing { get; }

        IBaseStation BaseStation { get; }
    }
}