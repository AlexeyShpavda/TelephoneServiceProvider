using System;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public class AnsweredCallEventArgs : EventArgs
    {
        public string PhoneNumberOfPersonAnsweredCall { get; set; }

        public DateTime CallStartTime { get; set; }

        public AnsweredCallEventArgs(string phoneNumberOfPersonAnsweredCall)
        {
            PhoneNumberOfPersonAnsweredCall = phoneNumberOfPersonAnsweredCall;
        }
    }
}