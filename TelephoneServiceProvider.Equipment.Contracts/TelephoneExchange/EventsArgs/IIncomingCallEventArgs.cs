namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public interface IIncomingCallEventArgs
    {
        string SenderPhoneNumber { get; set; }
    }
}