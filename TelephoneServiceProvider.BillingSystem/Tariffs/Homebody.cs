using System;
using TelephoneServiceProvider.BillingSystem.Tariffs.Abstract;

namespace TelephoneServiceProvider.BillingSystem.Tariffs
{
    public class Homebody : Tariff
    {
        public override decimal PricePerMinute { get; } = 0.05m;
    }
}