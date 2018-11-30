using System;

namespace TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract
{
    public interface ITariff
    {
        decimal PricePerMinute { get; }
    }
}