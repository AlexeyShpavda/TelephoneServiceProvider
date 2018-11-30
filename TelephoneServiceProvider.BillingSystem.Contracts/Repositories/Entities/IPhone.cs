using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;

namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities
{
    public interface IPhone
    {
        string PhoneNumber { get; }

        ITariff Tariff { get; }

        decimal Balance { get; }

        void IncreaseBalance(decimal amountOfMoney);

        void ReduceBalance(decimal amountOfMoney);
    }
}