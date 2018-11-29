namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public interface IOutgoingCallEventArgs
    {
        string SenderPhoneNumber { get; set; }

        string ReceiverPhoneNumber { get; set; }
    }
}