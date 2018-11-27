namespace TelephoneServiceProvider.BillingSystem.Tariffs.Abstract
{
    public abstract class Tariff
    {
        public abstract decimal CostPerMonth { get; }
    }
}