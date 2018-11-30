using System;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.BillingSystem.Repositories.Entities.Abstract;

namespace TelephoneServiceProvider.BillingSystem.Repositories.Entities
{
    public class UnansweredCall : Call, IUnansweredCall
    {
        public DateTime CallResetTime { get; set; }

        public UnansweredCall(string senderPhoneNumber, string receiverPhoneNumber, DateTime callResetTime)
            : base(senderPhoneNumber, receiverPhoneNumber)
        {
            CallResetTime = callResetTime;
        }

        public override string ToString()
        {
            return $"{base.ToString()} | {CallResetTime}";
        }
    }
}