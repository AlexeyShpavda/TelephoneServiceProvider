using System;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public interface IRejectedCallEventArgs
    {
        string PhoneNumberOfPersonRejectedCall { get; set; }

        DateTime CallRejectionTime { get; set; }
    }
}