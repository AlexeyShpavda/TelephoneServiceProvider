using System;

namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities
{
    public interface IUnansweredCall : ICall
    {
        DateTime CallResetTime { get; set; }
    }
}