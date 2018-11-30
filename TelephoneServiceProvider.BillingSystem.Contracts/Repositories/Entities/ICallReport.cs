using System.Collections.Generic;

namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities
{
    public interface ICallReport
    {
        IEnumerable<ICallInformation> CallInformation { get; }
    }
}