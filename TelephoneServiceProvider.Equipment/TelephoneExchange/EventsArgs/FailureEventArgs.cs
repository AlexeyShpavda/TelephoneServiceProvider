using System;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class FailureEventArgs : EventArgs, IFailureEventArgs
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