using TelephoneServiceProvider.BillingSystem.Tariffs.Abstract;

namespace TelephoneServiceProvider.BillingSystem.Tariffs
{
    public class Homebody : Tariff
    {
        public override decimal CostPerMonth { get; } = 15m;

        public int FreeMinutes { get; } = 1000;

        public double Megabytes { get; } = 3000;
    }
}