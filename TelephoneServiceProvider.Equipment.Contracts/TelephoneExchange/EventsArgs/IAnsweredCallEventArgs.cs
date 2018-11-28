using System;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public interface IAnsweredCallEventArgs
    {
        string PhoneNumberOfPersonAnsweredCall { get; set; }
        DateTime CallStartTime { get; set; }
    }
}