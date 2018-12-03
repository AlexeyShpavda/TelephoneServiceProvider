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

        ICallReport<TCallInfo, TCall> GetCallReport<TCallInfo, TCall>(string phoneNumber,
            Func<TCallInfo, bool> selectorCallInfo = null, Func<TCall, bool> selectorCall = null)
            where TCallInfo : ICallInformation<TCall>
            where TCall : ICall;

        void PutCallOnRecord(object sender, ICall e);

        void PutPhoneOnRecord(object sender, ContractConclusionEventArgs e);
    }
}