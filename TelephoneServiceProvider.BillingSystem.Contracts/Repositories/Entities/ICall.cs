namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities
{
    public interface ICall
    {
        string SenderPhoneNumber { get; }

        string ReceiverPhoneNumber { get; }
    }
}