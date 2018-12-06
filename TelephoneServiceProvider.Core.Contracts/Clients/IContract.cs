using System;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;

namespace TelephoneServiceProvider.Core.Contracts.Clients
{
    public interface IContract
    {
        Guid ContractNumber { get; }

        DateTime DateOfContract { get; }

        DateTime ContractExpirationDate { get; }

        ICompany Company { get; }

        IPassport IndividualPassport { get; }

        string PhoneNumber { get; }

        ITariff Tariff { get; }

        IClientEquipment ClientEquipment { get; }

        string Conditions { get; }
    }
}