﻿using System;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs
{
    public class CallEventArgs
    {
        public string SenderPhoneNumber { get; set; }

        public string ReceiverPhoneNumber { get; set; }

        public DateTime CallStartTime { get; set; }

        public DateTime CallEndTime { get; set; }

        public TimeSpan Duration => CallEndTime - CallStartTime;

        public CallEventArgs(string senderPhoneNumber, string receiverPhoneNumber, DateTime callStartTime, DateTime callEndTime)
        {
            SenderPhoneNumber = senderPhoneNumber;

            ReceiverPhoneNumber = receiverPhoneNumber;

            CallStartTime = callStartTime;

            CallEndTime = callEndTime;
        }
    }
}