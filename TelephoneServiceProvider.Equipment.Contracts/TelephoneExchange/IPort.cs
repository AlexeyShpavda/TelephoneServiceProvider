﻿using System;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange
{
    public interface IPort
    {
        event EventHandler<OutgoingCallEventArgs> NotifyStationOfOutgoingCall;

        event EventHandler<RejectedCallEventArgs> NotifyStationOfRejectionOfCall;

        event EventHandler<AnsweredCallEventArgs> NotifyStationOfAnsweredCall;

        event EventHandler<RejectedCallEventArgs> NotifyTerminalOfRejectionOfCall;

        event EventHandler<FailureEventArgs> NotifyTerminalOfFailure;

        event EventHandler<IncomingCallEventArgs> NotifyTerminalOfIncomingCall;

        string PhoneNumber { get; }

        PortStatus PortStatus { get; }

        void ConnectToTerminal();

        void DisconnectFromTerminal();

        void OutgoingCall(string receiverPhoneNumber);
    }
}