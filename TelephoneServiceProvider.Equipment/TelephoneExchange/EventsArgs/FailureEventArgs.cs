using System;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class FailureEventArgs : EventArgs
    {
        public string ReceiverPhoneNumber { get; set; }

        public FailureEventArgs(string receiverPhoneNumber)
        {
            ReceiverPhoneNumber = receiverPhoneNumber;
        }
    }
}