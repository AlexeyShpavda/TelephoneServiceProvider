using System.Collections.Generic;

namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities
{
    public interface ICallReport<out TCall> 
        where TCall : ICall
    {
        IEnumerable<ICallInformation<TCall>> CallInformation { get; }
    }
}