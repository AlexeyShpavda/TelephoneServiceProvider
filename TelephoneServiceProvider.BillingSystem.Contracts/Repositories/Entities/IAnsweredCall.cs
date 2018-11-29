using System;

namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities
{
    public interface IAnsweredCall : ICall
    {
        DateTime CallStartTime { get; set; }

        DateTime CallEndTime { get; set; }

        TimeSpan Duration { get; }
    }
}