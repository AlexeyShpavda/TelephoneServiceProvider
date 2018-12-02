using System;
using System.Collections.Generic;
using TelephoneServiceProvider.BillingSystem.Contracts;
using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;

namespace TelephoneServiceProvider.BillingSystem
{
    public class Billing : IBilling
    {
        public IEnumerable<ITariff> Tariffs { get; }

        public IBalanceOperation BalanceOperation { get; }

        private IBillingUnitOfWork Data { get; }

        private IPhoneManagement PhoneManagement { get; }

        private ICallManagement CallManagement { get; }

        public Billing(IEnumerable<ITariff> tariffs)
        {
            Data = new BillingUnitOfWork();
            PhoneManagement = new PhoneManagement(Data);
            BalanceOperation = new BalanceOperation(PhoneManagement);
            CallManagement = new CallManagement(Data, BalanceOperation, PhoneManagement);
            Tariffs = tariffs;
        }

        public void PutCallOnRecord(object sender, ICall call)
        {
            CallManagement.PutCallOnRecord(call);
        }

        public void PutPhoneOnRecord(object sender, ContractConclusionEventArgs e)
        {
            PhoneManagement.PutPhoneOnRecord(e.PhoneNumber, e.Tariff);
        }

        public ICallReport GetCallReport(string phoneNumber, Func<ICall, bool> selector = null)
        {
            return CallManagement.GetCallReport(phoneNumber, selector);
        }
    }
}