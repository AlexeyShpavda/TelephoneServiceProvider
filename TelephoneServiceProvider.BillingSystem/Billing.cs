using System;
using System.Collections.Generic;
using System.Linq;
using TelephoneServiceProvider.BillingSystem.Contracts;
using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;
using TelephoneServiceProvider.BillingSystem.Repositories.Entities;

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
            CallManagement = new CallManagement(Data, PhoneManagement);
            Tariffs = tariffs;
        }

        public void PutCallOnRecord(object sender, ICall call)
        {
            CallManagement.PutCallOnRecord(call);

            BalanceOperation.ReduceBalance(call.SenderPhoneNumber, CallManagement.CalculateCostOfCall(call));
        }

        public void PutPhoneOnRecord(object sender, ContractConclusionEventArgs e)
        {
            PhoneManagement.PutPhoneOnRecord(e.PhoneNumber, e.Tariff);
        }

        public ICallReport<TCall> GetCallReport<TCall>(string phoneNumber,
            Func<ICallInformation<TCall>, bool> selectorCallInfo = null, Func<TCall, bool> selectorCall = null)
            where TCall : ICall
        {
            var subscriberCalls = CallManagement.GetCallList(phoneNumber, selectorCall);

            IEnumerable<ICallInformation<TCall>> callInformationList;

            if (selectorCallInfo != null)
            {
                callInformationList = subscriberCalls.Select(call =>
                    new CallInformation<TCall>(call, CallManagement.CalculateCostOfCall(call))).Where(selectorCallInfo).ToList();
            }
            else
            {
                callInformationList = subscriberCalls.Select(call =>
                    new CallInformation<TCall>(call, CallManagement.CalculateCostOfCall(call))).ToList();
            }

            return new CallReport<TCall>(callInformationList);
        }
    }
}