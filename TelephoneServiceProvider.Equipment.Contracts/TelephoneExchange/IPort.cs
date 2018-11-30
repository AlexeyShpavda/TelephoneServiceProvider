﻿using System;
using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange
{
    public interface IPort
    {
        event EventHandler<IOutgoingCallEventArgs> NotifyStationOfOutgoingCall;

        event EventHandler<IRejectedCallEventArgs> NotifyStationOfRejectionOfCall;

        event EventHandler<IAnsweredCallEventArgs> NotifyStationOfAnsweredCall;

        event EventHandler<IRejectedCallEventArgs> NotifyTerminalOfRejectionOfCall;

        event EventHandler<IFailureEventArgs> NotifyTerminalOfFailure;

        event EventHandler<IIncomingCallEventArgs> NotifyTerminalOfIncomingCall;

        event EventHandler<ICheckBalanceEventArgs> NotifyTerminalOfLackOfMoneyInAccount;

        string PhoneNumber { get; }

        PortStatus PortStatus { get; }

        void ConnectToTerminal();

        void DisconnectFromTerminal();

        void OutgoingCall(string receiverPhoneNumber);
    }
}