﻿using System;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public class RejectedCallEventArgs : EventArgs
    {
        public string PhoneNumberOfPersonRejectedCall { get; set; }

        public DateTime CallRejectionTime { get; set; }

        public RejectedCallEventArgs(string phoneNumberOfPersonRejectedCall)
        {
            PhoneNumberOfPersonRejectedCall = phoneNumberOfPersonRejectedCall;
        }
    }
}