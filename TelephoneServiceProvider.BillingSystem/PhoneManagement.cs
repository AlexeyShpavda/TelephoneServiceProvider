using System;
using System.Linq;
using TelephoneServiceProvider.BillingSystem.Contracts;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;
using TelephoneServiceProvider.BillingSystem.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem
{
    internal class PhoneManagement : IPhoneManagement
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

        public void PutPhoneOnRecord(string phoneNumber, ITariff tariff)
        {
            Data.Phones.Add(new Phone(phoneNumber, tariff));
        }
    }
}