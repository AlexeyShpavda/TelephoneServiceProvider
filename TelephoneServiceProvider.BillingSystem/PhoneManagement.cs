using System;
using System.Linq;
using TelephoneServiceProvider.BillingSystem.Contracts;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem
{
    public class PhoneManagement : IPhoneManagement
    {
        private IBillingUnitOfWork Data { get; }

        public PhoneManagement(IBillingUnitOfWork data)
        {
            Data = data;
        }

        public IPhone GetPhoneOnNumber(string phoneNumber)
        {
            return Data.Phones.GetAll().FirstOrDefault(x => x.PhoneNumber == phoneNumber) ??
                   throw new Exception("Phone number doesn't exist");
        }
    }
}