using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class CheckBalanceEventArgs : ICheckBalanceEventArgs
    {
        public string PhoneNumber { get; set; }

        public bool IsAllowedCall { get; set; }

        public CheckBalanceEventArgs(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
    }
}
