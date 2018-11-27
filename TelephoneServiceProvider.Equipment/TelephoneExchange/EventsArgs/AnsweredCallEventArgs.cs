using System;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class AnsweredCallEventArgs
    {
        public string PhoneNumberOfPersonAnsweredCall { get; set; }

        public DateTime CallStartTime { get; set; }

        public AnsweredCallEventArgs(string phoneNumberOfPersonAnsweredCall)
        {
            PhoneNumberOfPersonAnsweredCall = phoneNumberOfPersonAnsweredCall;
        }
    }
}