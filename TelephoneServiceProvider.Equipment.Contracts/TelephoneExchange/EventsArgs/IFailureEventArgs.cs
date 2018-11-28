namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public interface IFailureEventArgs
    {
        string ReceiverPhoneNumber { get; set; }
    }
}