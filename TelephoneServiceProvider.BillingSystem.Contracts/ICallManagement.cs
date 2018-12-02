using System;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Contracts
{
    public interface ICallManagement
    {
        void PutCallOnRecord(ICall call);

        ICallReport GetCallReport(string phoneNumber, Func<ICall, bool> selector = null);

        decimal CalculateCostOfCall(ICall call);
    }
}