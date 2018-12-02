using TelephoneServiceProvider.BillingSystem.Contracts;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem
{
    public class BalanceOperation : IBalanceOperation
    {
        private IPhoneManagement PhoneManagement { get; }

        public BalanceOperation(IPhoneManagement phoneManagement)
        {
            PhoneManagement = phoneManagement;
        }

        public decimal GetBalance(string phoneNumber)
        {
            var phone = PhoneManagement.GetPhoneOnNumber(phoneNumber);

            return phone.Balance;
        }

        public void IncreaseBalance(string phoneNumber, decimal amountOfMoney)
        {
            var phone = PhoneManagement.GetPhoneOnNumber(phoneNumber);

            phone?.IncreaseBalance(amountOfMoney);
        }

        public void ReduceBalance(string phoneNumber, decimal amountOfMoney)
        {
            var phone = PhoneManagement.GetPhoneOnNumber(phoneNumber);

            phone?.ReduceBalance(amountOfMoney);
        }

        public decimal CalculateCostOfCall(ICall call)
        {
            if (!(call is IAnsweredCall answeredCall)) return 0;

            var phone = PhoneManagement.GetPhoneOnNumber(answeredCall.SenderPhoneNumber);
            var duration = answeredCall.Duration;
            var callDurationInSeconds = duration.Hours * 3600 + duration.Minutes * 60 + duration.Seconds;
            var pricePerSecond = phone.Tariff.PricePerMinute / 60;
            var callCost = callDurationInSeconds * pricePerSecond;

            return callCost;
        }
    }
}