namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities
{
    public interface ICallInformation<out T> where T : ICall
    {
        T Call { get; }

        decimal CallCost { get; }
    }
}