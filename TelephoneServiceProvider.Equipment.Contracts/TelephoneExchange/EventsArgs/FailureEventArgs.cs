using System;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public class FailureEventArgs : EventArgs
    {
        public string PhoneNumber { get; set; }

        public FailureType FailureType { get; set; }

        public FailureEventArgs(string phoneNumber, FailureType failureType)
        {
            PhoneNumber = phoneNumber;
            FailureType = failureType;
        }
    }
}