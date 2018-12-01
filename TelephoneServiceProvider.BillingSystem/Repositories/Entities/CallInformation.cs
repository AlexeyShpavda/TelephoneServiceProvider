using System;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Repositories.Entities
{
    public class CallInformation : ICallInformation
    {
        public ICall Call { get; }

        public decimal CallCost { get; }

        public CallInformation(ICall call, decimal callCost)
        {
            Call = call;
            CallCost = callCost;
        }

        public override string ToString()
        {
            return $"{Call} | Call cost: {Math.Round(CallCost, 3)}";
        }
    }
}