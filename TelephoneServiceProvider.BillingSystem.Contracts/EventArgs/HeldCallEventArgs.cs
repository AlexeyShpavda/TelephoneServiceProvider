using System;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Contracts.EventArgs
{
    public class HeldCallEventArgs : CallEventArgs, IAnsweredCall
    {
        public DateTime CallStartTime { get; set; }

        public DateTime CallEndTime { get; set; }

        public TimeSpan Duration => CallEndTime - CallStartTime;

        public HeldCallEventArgs(string senderPhoneNumber, string receiverPhoneNumber)
            : base(senderPhoneNumber, receiverPhoneNumber)
        {
        }

        public HeldCallEventArgs(string senderPhoneNumber, string receiverPhoneNumber, DateTime callStartTime, DateTime callEndTime)
            : base(senderPhoneNumber, receiverPhoneNumber)
        {
            CallStartTime = callStartTime;

            CallEndTime = callEndTime;
        }
    }
}