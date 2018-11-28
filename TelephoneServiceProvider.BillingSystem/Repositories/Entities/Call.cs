using System;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Repositories.Entities
{
    public class Call : ICall
    {
        public string SenderPhoneNumber { get; set; }

        public string ReceiverPhoneNumber { get; set; }

        public DateTime CallStartTime { get; set; }

        public DateTime CallEndTime { get; set; }

        public TimeSpan Duration => CallEndTime - CallStartTime;

        public Call(string senderPhoneNumber, string receiverPhoneNumber)
        {
            SenderPhoneNumber = senderPhoneNumber;

            ReceiverPhoneNumber = receiverPhoneNumber;
        }

        public Call(string senderPhoneNumber, string receiverPhoneNumber, DateTime callStartTime, DateTime callEndTime)
            : this(senderPhoneNumber, receiverPhoneNumber)
        {
            CallStartTime = callStartTime;

            CallEndTime = callEndTime;
        }

        public override string ToString()
        {
            return $"{SenderPhoneNumber} | {ReceiverPhoneNumber} | {CallStartTime} | {CallEndTime} | {Duration}";
        }
    }
}