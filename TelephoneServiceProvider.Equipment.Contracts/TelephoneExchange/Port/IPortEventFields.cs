using System;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port
{
    public interface IPortEventFields
    {
        event EventHandler<OutgoingCallEventArgs> NotifyStationOfOutgoingCall;

        event EventHandler<RejectedCallEventArgs> NotifyStationOfRejectionOfCall;

        event EventHandler<AnsweredCallEventArgs> NotifyStationOfAnsweredCall;

        event EventHandler<RejectedCallEventArgs> NotifyTerminalOfRejectionOfCall;

        event EventHandler<FailureEventArgs> NotifyTerminalOfFailure;

        event EventHandler<IncomingCallEventArgs> NotifyTerminalOfIncomingCall;
    }
}