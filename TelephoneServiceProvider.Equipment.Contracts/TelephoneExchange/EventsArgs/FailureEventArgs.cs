using System;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public class FailureEventArgs : EventArgs
    {
        public string ReceiverPhoneNumber { get; set; }

        public FailureType FailureType { get; set; }

        public FailureEventArgs(string receiverPhoneNumber, FailureType failureType)
        {
            ReceiverPhoneNumber = receiverPhoneNumber;
            FailureType = failureType;
        }
    }
}