using System;

namespace TelephoneServiceProvider.Core.Clients
{
    public class Contract
    {
        public Guid ContractNumber { get; private set; }

        public Company Company { get; private set; }

        public Passport IndividualPassport { get; private set; }

        public string Conditions { get; private set; }

        public Contract(Company company, Passport individualPassport, string conditions)
        {
            ContractNumber = Guid.NewGuid();
            Company = company;
            IndividualPassport = individualPassport;
            Conditions = conditions;
        }
    }
}