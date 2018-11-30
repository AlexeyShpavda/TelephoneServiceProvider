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
        public IEnumerable<ITariff> Tariffs { get; private set; }

        private IBillingUnitOfWork Data { get; set; }

        public Billing(IEnumerable<ITariff> tariffs)
        {
            Data = new BillingUnitOfWork();
            Tariffs = tariffs;
        }

        public void PutCallOnRecord(object sender, ICall call)
        {
            switch (call)
            {
                case IAnsweredCall answeredCall:
                {
                    Data.Calls.Add(new AnsweredCall(
                        answeredCall.SenderPhoneNumber,
                        answeredCall.ReceiverPhoneNumber,
                        answeredCall.CallStartTime,
                        answeredCall.CallEndTime));

                    ReduceBalance(call.SenderPhoneNumber, CalculateCostOfCall(call));
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

        public void PutPhoneOnRecord(object sender, IBillingSystemEventArgs e)
        {
            var newPhone = new Phone(e.PhoneNumber, e.Tariff);
            Data.Phones.Add(newPhone);
        }

        public decimal GetBalance(string phoneNumber)
        {
            var phone = GetPhoneOnNumber(phoneNumber);

            return phone.Balance;
        }

        private void ReduceBalance(string phoneNumber, decimal amountOfMoney)
        {
            var phone = GetPhoneOnNumber(phoneNumber);

            phone?.ReduceBalance(amountOfMoney);
        }

        public void IncreaseBalance(string phoneNumber, decimal amountOfMoney)
        {
            var phone = GetPhoneOnNumber(phoneNumber);

            phone?.IncreaseBalance(amountOfMoney);
        }

        public void CheckPossibilityOfCall(object sender, ICheckBalanceEventArgs e)
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
                new CallInformation(call, CalculateCostOfCall(call))));
        }

        public decimal CalculateCostOfCall(ICall call)
        {
            if (!(call is IAnsweredCall answeredCall)) return 0;

            var phone = GetPhoneOnNumber(answeredCall.SenderPhoneNumber);
            var duration = answeredCall.Duration;
            var callDurationInSeconds = duration.Hours * 3600 + duration.Minutes * 60 + duration.Seconds;
            var pricePerSecond = phone.Tariff.PricePerMinute / 60;
            var callCost = callDurationInSeconds * pricePerSecond;

            return callCost;
        }

        public IPhone GetPhoneOnNumber(string phoneNumber)
        {
            return Data.Phones.GetAll().FirstOrDefault(x => x.PhoneNumber == phoneNumber) ??
                   throw new Exception("Phone number don't exist");
        }
    }
}