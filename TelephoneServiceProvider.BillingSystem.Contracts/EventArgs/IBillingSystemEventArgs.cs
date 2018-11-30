using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;

namespace TelephoneServiceProvider.BillingSystem.Contracts.EventArgs
{
    public interface IBillingSystemEventArgs
    {
        string PhoneNumber { get; set; }

        ITariff Tariff { get; }
    }
}