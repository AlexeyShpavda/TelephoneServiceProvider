using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;

namespace TelephoneServiceProvider.BillingSystem.Contracts
{
    public interface IBalanceOperation
    {
        decimal GetBalance(string phoneNumber);

        void IncreaseBalance(string phoneNumber, decimal amountOfMoney);

        void ReduceBalance(string phoneNumber, decimal amountOfMoney);

        void CheckPossibilityOfCall(object sender, CheckBalanceEventArgs e);
    }
}