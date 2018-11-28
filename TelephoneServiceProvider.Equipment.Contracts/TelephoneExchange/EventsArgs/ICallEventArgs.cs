using System;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public interface ICallEventArgs
    {
        string SenderPhoneNumber { get; set; }
        string ReceiverPhoneNumber { get; set; }
        DateTime CallStartTime { get; set; }
        DateTime CallEndTime { get; set; }
        TimeSpan Duration { get; }
    }
}