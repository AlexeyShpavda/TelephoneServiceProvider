using System;
using System.Linq;
using TelephoneServiceProvider.BillingSystem.Contracts;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem
{
    public class BalanceOperation : IBalanceOperation
    {
        private IBillingUnitOfWork Data { get; }

        public BalanceOperation(IBillingUnitOfWork data)
        {
            Data = data;
        }

        public decimal GetBalance(string phoneNumber)
        {
            var phone = GetPhoneOnNumber(phoneNumber);

            return phone.Balance;
        }

        public void IncreaseBalance(string phoneNumber, decimal amountOfMoney)
        {
            var phone = GetPhoneOnNumber(phoneNumber);

            phone?.IncreaseBalance(amountOfMoney);
        }

        public void ReduceBalance(string phoneNumber, decimal amountOfMoney)
        {
            var phone = GetPhoneOnNumber(phoneNumber);

            phone?.ReduceBalance(amountOfMoney);
        }

        public decimal CalculateCostOfCall(ICall call)
        {
            if (!(call is IAnsweredCall answeredCall)) return 0;

            var phone = GetPhoneOnNumber(answeredCall.SenderPhoneNumber);
            var duration = answeredCall.Duration;
            var callDurationInSeconds = duration.Hours * 3600 + duration.Minutes * 60 + duration.Seconds;
            var pricePerSecond = phone.Tariff.PricePerMinute / 60;
            var callCost = callDurationInSeconds * pricePerSecond;

            return callCost;
        }

        public IPhone GetPhoneOnNumber(string phoneNumber)
        {
            return Data.Phones.GetAll().FirstOrDefault(x => x.PhoneNumber == phoneNumber) ??
                   throw new Exception("Phone number don't exist");
        }
    }
}