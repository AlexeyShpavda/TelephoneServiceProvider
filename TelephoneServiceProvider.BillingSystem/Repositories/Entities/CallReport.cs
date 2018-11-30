using System.Collections.Generic;
using System.Text;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Repositories.Entities
{
    public class CallReport : ICallReport
    {
        public IEnumerable<ICallInformation> CallInformation { get; private set; }

        public CallReport(IEnumerable<ICallInformation> callInformation)
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