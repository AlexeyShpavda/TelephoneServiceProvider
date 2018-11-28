using System;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class IncomingCallEventArgs : EventArgs, IIncomingCallEventArgs
    {
        public string SenderPhoneNumber { get; set; }

        public IncomingCallEventArgs(string senderPhoneNumber)
        {
            SenderPhoneNumber = senderPhoneNumber;
        }
    }
}