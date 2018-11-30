using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;

namespace TelephoneServiceProvider.Core.EventArgs
{
    public class BillingSystemEventArgs : System.EventArgs, IBillingSystemEventArgs
    {
        public string PhoneNumber { get; set; }

        public ITariff Tariff { get; }

        public BillingSystemEventArgs(string phoneNumber, ITariff tariff)
        {
            PhoneNumber = phoneNumber;

            Tariff = tariff;
        }
    }
}