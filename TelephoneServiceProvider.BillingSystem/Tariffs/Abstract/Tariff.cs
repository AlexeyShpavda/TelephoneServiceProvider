using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;

namespace TelephoneServiceProvider.BillingSystem.Tariffs.Abstract
{
    public abstract class Tariff : ITariff
    {
        public abstract decimal CostPerMonth { get; }
    }
}