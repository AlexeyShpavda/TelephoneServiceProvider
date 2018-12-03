using System;
using System.Collections.Generic;
using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;

namespace TelephoneServiceProvider.BillingSystem.Contracts
{
    public interface IBilling
    {
        IEnumerable<ITariff> Tariffs { get; }

        IBalanceOperation BalanceOperation { get; }

        ICallReport<TCall> GetCallReport<TCall>(string phoneNumber,
            Func<ICallInformation<TCall>, bool> selectorCallInfo = null, Func<TCall, bool> selectorCall = null)
            where TCall : ICall;

        void PutCallOnRecord(object sender, ICall e);

        void PutPhoneOnRecord(object sender, ContractConclusionEventArgs e);
    }
}