using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Repositories.Entities
{
    public class Phone : IPhone
    {
        public string PhoneNumber { get; private set; }

        public decimal Balance { get; private set; }

        public Phone(string phoneNumber, decimal balance = 0)
        {
            PhoneNumber = phoneNumber;
            Balance = balance;
        }

        public void ChangeBalanceToAmount(decimal amountOfMoney)
        {
            Balance += amountOfMoney;
        }
    }
}