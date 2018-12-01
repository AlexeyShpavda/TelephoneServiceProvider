using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Contracts
{
    public interface IBalanceOperation
    {
        decimal GetBalance(string phoneNumber);

        void IncreaseBalance(string phoneNumber, decimal amountOfMoney);

        void ReduceBalance(string phoneNumber, decimal amountOfMoney);

        decimal CalculateCostOfCall(ICall call);
    }
}