using System.Collections.Generic;
using System.Text;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Repositories.Entities
{
    public class CallReport<TCall> : ICallReport<TCall>
        where TCall : ICall
    {
        public IEnumerable<ICallInformation<TCall>> CallInformation { get; }

        public CallReport(IEnumerable<ICallInformation<TCall>> callInformation)
        {
            CallInformation = callInformation;
        }

        public override string ToString()
        {
            var callReportAsString = new StringBuilder();

            foreach (var callInformation in CallInformation)
            {
                callReportAsString.Append($"{callInformation}\n");
            }

            return callReportAsString.ToString();
        }
    }
}