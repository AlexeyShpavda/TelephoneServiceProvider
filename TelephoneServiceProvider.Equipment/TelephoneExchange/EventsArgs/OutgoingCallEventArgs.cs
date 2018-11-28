using System;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class OutgoingCallEventArgs : EventArgs, IOutgoingCallEventArgs
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