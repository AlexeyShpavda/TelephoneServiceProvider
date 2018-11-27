using System;
using TelephoneServiceProvider.BillingSystem.Tariffs.Abstract;

namespace TelephoneServiceProvider.Core.Clients
{
    public class Contract
    {
        public Guid ContractNumber { get; private set; }

        public Company Company { get; private set; }

        public Passport IndividualPassport { get; private set; }

        public string PhoneNumber { get; private set; }

        public Tariff Tariff { get; private set; }

        public ClientEquipment ClientEquipment { get; private set; }

        public string Conditions { get; private set; }

        public Contract(Company company, Passport individualPassport, string phoneNumber, Tariff tariff, ClientEquipment clientEquipment, string conditions)
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