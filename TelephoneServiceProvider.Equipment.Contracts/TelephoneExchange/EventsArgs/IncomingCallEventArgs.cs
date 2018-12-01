using System;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public class IncomingCallEventArgs : EventArgs
    {
        public string SenderPhoneNumber { get; set; }

        public IncomingCallEventArgs(string senderPhoneNumber)
        {
            SenderPhoneNumber = senderPhoneNumber;
        }
    }
}