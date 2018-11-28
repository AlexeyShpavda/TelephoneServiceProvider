using System;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class RejectedCallEventArgs : EventArgs, IRejectedCallEventArgs
    {
        public string PhoneNumberOfPersonRejectedCall { get; set; }

        public DateTime CallRejectionTime { get; set; }

        public RejectedCallEventArgs(string phoneNumberOfPersonRejectedCall)
        {
            PhoneNumberOfPersonRejectedCall = phoneNumberOfPersonRejectedCall;
        }
    }
}