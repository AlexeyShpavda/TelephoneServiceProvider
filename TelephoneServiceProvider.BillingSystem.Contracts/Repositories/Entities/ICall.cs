using System;

namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities
{
    public interface ICall
    {
        string SenderPhoneNumber { get; set; }

        string ReceiverPhoneNumber { get; set; }

        DateTime CallStartTime { get; set; }

        DateTime CallEndTime { get; set; }

        TimeSpan Duration { get; }
    }
}