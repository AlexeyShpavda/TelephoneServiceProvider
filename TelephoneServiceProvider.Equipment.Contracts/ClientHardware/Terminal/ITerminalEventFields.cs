using System;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.Contracts.ClientHardware.Terminal
{
    public interface ITerminalEventFields
    {
        event EventHandler<ConnectionEventArgs> ConnectedToPort;

        event EventHandler<ConnectionEventArgs> DisconnectedFromPort;

        event EventHandler<OutgoingCallEventArgs> NotifyPortAboutOutgoingCall;

        event EventHandler<RejectedCallEventArgs> NotifyPortAboutRejectionOfCall;

        event EventHandler<AnsweredCallEventArgs> NotifyPortAboutAnsweredCall;
    }
}