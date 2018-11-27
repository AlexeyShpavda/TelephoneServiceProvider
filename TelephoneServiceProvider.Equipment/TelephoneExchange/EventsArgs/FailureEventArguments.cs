using System;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class FailureEventArguments : EventArgs
    {
        public string ReceiverPhoneNumber { get; set; }

        public FailureEventArguments(string receiverPhoneNumber)
        {
            ReceiverPhoneNumber = receiverPhoneNumber;
        }
    }
}