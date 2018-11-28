using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;

namespace TelephoneServiceProvider.Core.EventArgs
{
    public class BillingSystemEventArgs : System.EventArgs, IBillingSystemEventArgs
    {
        public string PhoneNumber { get; set; }

        public BillingSystemEventArgs(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
    }
}