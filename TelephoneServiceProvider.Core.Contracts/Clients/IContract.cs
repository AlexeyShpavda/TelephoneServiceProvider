using System;

namespace TelephoneServiceProvider.Core.Contracts.Clients
{
    public interface IContract
    {
        Guid ContractNumber { get; }

        ICompany Company { get; }

        IPassport IndividualPassport { get; }

        string PhoneNumber { get; }

        ITariff Tariff { get; }

        IClientEquipment ClientEquipment { get; }

        string Conditions { get; }
    }
}