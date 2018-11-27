using System;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class IncomingCallEventArguments : EventArgs
    {
        public string SenderPhoneNumber { get; set; }

        public IncomingCallEventArguments(string senderPhoneNumber)
        {
            SenderPhoneNumber = senderPhoneNumber;
        }
    }
}