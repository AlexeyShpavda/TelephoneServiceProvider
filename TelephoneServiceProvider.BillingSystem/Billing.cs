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

        private IBillingUnitOfWork Data { get; }

        public IBalanceOperation BalanceOperation { get; }

        public Billing(IEnumerable<ITariff> tariffs)
        {
            Data = new BillingUnitOfWork();
            BalanceOperation = new BalanceOperation(Data);
            Tariffs = tariffs;
        }

        public void PutCallOnRecord(object sender, ICall call)
        {
            switch (call)
            {
                case IAnsweredCall answeredCall:
                    {
                        Data.Calls.Add(new HeldCall(
                            answeredCall.SenderPhoneNumber,
                            answeredCall.ReceiverPhoneNumber,
                            answeredCall.CallStartTime,
                            answeredCall.CallEndTime));

                        BalanceOperation.ReduceBalance(call.SenderPhoneNumber,
                            BalanceOperation.CalculateCostOfCall(call));
                    }
                    break;

                case IUnansweredCall unansweredCall:
                    {
                        Data.Calls.Add(new UnansweredCall(
                            unansweredCall.SenderPhoneNumber,
                            unansweredCall.ReceiverPhoneNumber,
                            unansweredCall.CallResetTime));
                    }
                    break;
            }
        }

        public void PutPhoneOnRecord(object sender, ContractConclusionEventArgs e)
        {
            var newPhone = new Phone(e.PhoneNumber, e.Tariff);
            Data.Phones.Add(newPhone);
        }

        public void CheckPossibilityOfCall(object sender, CheckBalanceEventArgs e)
        {
            var phone = GetPhoneOnNumber(e.PhoneNumber);

            e.IsAllowedCall = phone.Balance >= 0;
        }

        public ICallReport GetCallReport(string phoneNumber, Func<ICall, bool> selector = null)
        {
            var subscriberCalls = selector != null
                ? Data.Calls.GetAll()
                    .Where(x => x.SenderPhoneNumber == phoneNumber || x.ReceiverPhoneNumber == phoneNumber)
                    .Where(selector).ToList()
                : Data.Calls.GetAll()
                    .Where(x => x.SenderPhoneNumber == phoneNumber || x.ReceiverPhoneNumber == phoneNumber);

            return new CallReport(subscriberCalls.Select(call =>
                new CallInformation(call,
                    BalanceOperation.CalculateCostOfCall(call))));
        }

        
        public IPhone GetPhoneOnNumber(string phoneNumber)
        {
            return Data.Phones.GetAll().FirstOrDefault(x => x.PhoneNumber == phoneNumber) ??
                   throw new Exception("Phone number don't exist");
        }
    }
}