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

        ICallReport GetCallReport<T>(string phoneNumber, Func<T, bool> selector = null) where T : ICall;

        void PutCallOnRecord(object sender, ICall e);

        void PutPhoneOnRecord(object sender, ContractConclusionEventArgs e);
    }
}