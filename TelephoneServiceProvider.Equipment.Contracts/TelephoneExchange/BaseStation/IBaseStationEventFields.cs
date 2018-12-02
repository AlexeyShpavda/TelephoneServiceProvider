using System;
using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.BaseStation
{
    public interface IBaseStationEventFields
    {
        event EventHandler<IncomingCallEventArgs> NotifyPortOfIncomingCall;

        event EventHandler<RejectedCallEventArgs> NotifyPortOfRejectionOfCall;

        event EventHandler<FailureEventArgs> NotifyPortOfFailure;

        event EventHandler<CallEventArgs> NotifyBillingSystemAboutCallEnd;

        event EventHandler<CheckBalanceEventArgs> CheckBalanceInBillingSystem;
    }
}