namespace TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract
{
    public interface ITariff
    {
        decimal CostPerMonth { get; }
    }
}