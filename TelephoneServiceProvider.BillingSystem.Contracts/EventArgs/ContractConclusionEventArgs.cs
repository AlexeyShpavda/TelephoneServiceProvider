using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;

namespace TelephoneServiceProvider.BillingSystem.Contracts.EventArgs
{
    public class ContractConclusionEventArgs : System.EventArgs
    {
        public string PhoneNumber { get; set; }

        public ITariff Tariff { get; }

        public ContractConclusionEventArgs(string phoneNumber, ITariff tariff)
        {
            PhoneNumber = phoneNumber;

            Tariff = tariff;
        }
    }
}