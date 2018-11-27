using System;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class OutgoingCallEventArgs : EventArgs
    {
        public string SenderPhoneNumber { get; set; }

        public string ReceiverPhoneNumber { get; set; }

        public OutgoingCallEventArgs(string senderPhoneNumber, string receiverPhoneNumber)
        {
            SenderPhoneNumber = senderPhoneNumber;
            ReceiverPhoneNumber = receiverPhoneNumber;
        }
    }
}