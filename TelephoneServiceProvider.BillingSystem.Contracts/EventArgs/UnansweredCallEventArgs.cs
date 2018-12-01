using System;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Contracts.EventArgs
{
    public class UnansweredCallEventArgs : CallEventArgs, IUnansweredCall
    {
        public DateTime CallResetTime { get; set; }

        public UnansweredCallEventArgs(string senderPhoneNumber, string receiverPhoneNumber, DateTime callResetTime)
            : base(senderPhoneNumber, receiverPhoneNumber)
        {
            CallResetTime = callResetTime;
        }
    }
}