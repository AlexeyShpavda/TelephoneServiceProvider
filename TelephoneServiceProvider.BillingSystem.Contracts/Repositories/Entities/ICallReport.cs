using System.Collections.Generic;

namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities
{
    public interface ICallReport<out TCallInfo, out TCall> 
        where TCallInfo : ICallInformation<TCall>
        where TCall :ICall
    {
        IEnumerable<TCallInfo> CallInformation { get; }
    }
}