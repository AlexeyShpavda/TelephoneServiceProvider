using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;

namespace TelephoneServiceProvider.BillingSystem.Repositories.Entities
{
    public class Phone : IPhone
    {
        public string PhoneNumber { get; private set; }

        public ITariff Tariff { get; private set; }

        public decimal Balance { get; private set; }

        public Phone(string phoneNumber, ITariff tariff, decimal balance = 0)
        {
            PhoneNumber = phoneNumber;
            Tariff = tariff;
            Balance = balance;
        }

        public void IncreaseBalance(decimal amountOfMoney)
        {
            Balance += amountOfMoney;
        }

        public void ReduceBalance(decimal amountOfMoney)
        {
            Balance -= amountOfMoney;
        }
    }
}