using System;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;
using TelephoneServiceProvider.Core.Contracts;
using TelephoneServiceProvider.Core.Contracts.Clients;

namespace TelephoneServiceProvider.Core.Clients
{
    public class Contract : IContract
    {
        public Guid ContractNumber { get; }

        public DateTime DateOfContract { get; }

        public ICompany Company { get; }

        public IPassport IndividualPassport { get; }

        public string PhoneNumber { get; }

        public ITariff Tariff { get; }

        public IClientEquipment ClientEquipment { get; }

        public string Conditions { get; }

        public Contract(ICompany company, IPassport individualPassport, string phoneNumber, ITariff tariff,
            IClientEquipment clientEquipment, string conditions)
        {
            ContractNumber = Guid.NewGuid();
            DateOfContract = DateTime.Now;
            Company = company;
            IndividualPassport = individualPassport;
            PhoneNumber = phoneNumber;
            Tariff = tariff;
            ClientEquipment = clientEquipment;
            Conditions = conditions;
        }
    }
}