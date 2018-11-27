using System;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class RejectedCallEventArgs : EventArgs
    {
        public string PhoneNumberOfPersonRejectedCall { get; set; }

        public RejectedCallEventArgs(string phoneNumberOfPersonRejectedCall)
        {
            PhoneNumberOfPersonRejectedCall = phoneNumberOfPersonRejectedCall;
        }
    }
}