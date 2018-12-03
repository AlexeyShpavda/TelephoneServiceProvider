using System;
using System.Collections.Generic;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Contracts
{
    public interface ICallManagement
    {
        void PutCallOnRecord(ICall call);

        IEnumerable<T> GetCallList<T>(string phoneNumber, Func<T, bool> selector = null) where T : ICall;

        decimal CalculateCostOfCall(ICall call);
    }
}