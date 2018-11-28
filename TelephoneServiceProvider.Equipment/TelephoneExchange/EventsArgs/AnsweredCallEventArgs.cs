using System;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class AnsweredCallEventArgs : IAnsweredCallEventArgs
    {
        public string PhoneNumberOfPersonAnsweredCall { get; set; }

        public DateTime CallStartTime { get; set; }

        public AnsweredCallEventArgs(string phoneNumberOfPersonAnsweredCall)
        {
            PhoneNumberOfPersonAnsweredCall = phoneNumberOfPersonAnsweredCall;
        }
    }
}