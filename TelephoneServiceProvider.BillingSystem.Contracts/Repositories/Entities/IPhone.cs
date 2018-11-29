namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities
{
    public interface IPhone
    {
        string PhoneNumber { get; }

        decimal Balance { get; }

        void ChangeBalanceToAmount(decimal amountOfMoney);
    }
}