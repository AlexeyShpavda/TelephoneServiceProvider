using TelephoneServiceProvider.BillingSystem.Contracts;
using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;

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

        public void CheckPossibilityOfCall(object sender, CheckBalanceEventArgs e)
        {
            var phone = PhoneManagement.GetPhoneOnNumber(e.PhoneNumber);

            e.IsAllowedCall = phone.Balance >= 0;
        }
    }
}