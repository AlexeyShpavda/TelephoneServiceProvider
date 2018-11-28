using System;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;
using TelephoneServiceProvider.Core.Contracts;
using TelephoneServiceProvider.Core.Contracts.Clients;

namespace TelephoneServiceProvider.Core.Clients
{
    public class Contract : IContract
    {
        public Guid ContractNumber { get; private set; }

        public ICompany Company { get; private set; }

        public IPassport IndividualPassport { get; private set; }

        public string PhoneNumber { get; private set; }

        public ITariff Tariff { get; private set; }

        public IClientEquipment ClientEquipment { get; private set; }

        public string Conditions { get; private set; }

        public Contract(ICompany company, IPassport individualPassport, string phoneNumber, ITariff tariff, IClientEquipment clientEquipment, string conditions)
        {
            ContractNumber = Guid.NewGuid();
            Company = company;
            IndividualPassport = individualPassport;
            PhoneNumber = phoneNumber;
            Tariff = tariff;
            ClientEquipment = clientEquipment;
            Conditions = conditions;
        }
    }
}