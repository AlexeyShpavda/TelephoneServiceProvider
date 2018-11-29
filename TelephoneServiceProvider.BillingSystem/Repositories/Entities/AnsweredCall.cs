using System;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.BillingSystem.Repositories.Entities.Abstract;

namespace TelephoneServiceProvider.BillingSystem.Repositories.Entities
{
    public class AnsweredCall : Call, IAnsweredCall
    {
        public DateTime CallStartTime { get; set; }

        public DateTime CallEndTime { get; set; }

        public TimeSpan Duration => CallEndTime - CallStartTime;

        public AnsweredCall(string senderPhoneNumber, string receiverPhoneNumber) 
            : base(senderPhoneNumber, receiverPhoneNumber)
        {
        }

        public AnsweredCall(string senderPhoneNumber, string receiverPhoneNumber, DateTime callStartTime, DateTime callEndTime)
            : base(senderPhoneNumber, receiverPhoneNumber)
        {
            CallStartTime = callStartTime;

            CallEndTime = callEndTime;
        }
    }
}