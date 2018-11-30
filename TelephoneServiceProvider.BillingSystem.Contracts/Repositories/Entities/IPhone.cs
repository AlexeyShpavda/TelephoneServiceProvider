using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;

namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities
{
    public interface IPhone
    {
        string PhoneNumber { get; }

        ITariff Tariff { get; }

        decimal Balance { get; }

        void ChangeBalanceToAmount(decimal amountOfMoney);
    }
}