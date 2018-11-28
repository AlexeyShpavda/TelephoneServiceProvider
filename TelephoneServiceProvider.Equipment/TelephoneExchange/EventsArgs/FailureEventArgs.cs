using System;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class FailureEventArgs : EventArgs, IFailureEventArgs
    {
        public string ReceiverPhoneNumber { get; set; }

        public FailureEventArgs(string receiverPhoneNumber)
        {
            ReceiverPhoneNumber = receiverPhoneNumber;
        }
    }
}