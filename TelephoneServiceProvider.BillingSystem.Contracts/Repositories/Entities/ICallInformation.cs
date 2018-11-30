namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities
{
    public interface ICallInformation
    {
        ICall Call { get; }
        decimal CallCost { get; }
    }
}