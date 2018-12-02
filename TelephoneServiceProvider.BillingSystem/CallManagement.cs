using System;
using System.Linq;
using TelephoneServiceProvider.BillingSystem.Contracts;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.BillingSystem.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem
{
    public class CallManagement : ICallManagement
    {
        private IBillingUnitOfWork Data { get; }

        private IBalanceOperation BalanceOperation { get; }

        private IPhoneManagement PhoneManagement { get; }

        public CallManagement(IBillingUnitOfWork data, IBalanceOperation balanceOperation, IPhoneManagement phoneManagement)
        {
            Data = data;
            BalanceOperation = balanceOperation;
            PhoneManagement = phoneManagement;
        }

        public void PutCallOnRecord(ICall call)
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

                    BalanceOperation.ReduceBalance(call.SenderPhoneNumber, CalculateCostOfCall(call));
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

        public ICallReport GetCallReport(string phoneNumber, Func<ICall, bool> selector = null)
        {
            var subscriberCalls = selector != null
                ? Data.Calls.GetAll()
                    .Where(x => x.SenderPhoneNumber == phoneNumber || x.ReceiverPhoneNumber == phoneNumber)
                    .Where(selector).ToList()
                : Data.Calls.GetAll()
                    .Where(x => x.SenderPhoneNumber == phoneNumber || x.ReceiverPhoneNumber == phoneNumber);

            return new CallReport(subscriberCalls.Select(call =>
                new CallInformation(call, CalculateCostOfCall(call))));
        }

        public decimal CalculateCostOfCall(ICall call)
        {
            if (!(call is IAnsweredCall answeredCall)) return 0;

            var phone = PhoneManagement.GetPhoneOnNumber(answeredCall.SenderPhoneNumber);
            var duration = answeredCall.Duration;
            var callDurationInSeconds = duration.Hours * 3600 + duration.Minutes * 60 + duration.Seconds;
            var pricePerSecond = phone.Tariff.PricePerMinute / 60;
            var callCost = callDurationInSeconds * pricePerSecond;

            return callCost;
        }
    }
}