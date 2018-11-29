﻿using System;
using TelephoneServiceProvider.Equipment.Contracts.ClientHardware.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.Contracts.ClientHardware
{
    public interface ITerminal
    {
        event EventHandler<IRejectedCallEventArgs> NotifyPortAboutRejectionOfCall;

        event EventHandler<IAnsweredCallEventArgs> NotifyPortAboutAnsweredCall;

        Guid SerialNumber { get; }

        IPort Port { get; set; }

        TerminalStatus TerminalStatus { get; }

        void SetDisplayMethod(Action<string> action);

        void ConnectToPort(IPort port);

        void DisconnectFromPort();

        void Call(string receiverPhoneNumber);

        void Answer();

        void Reject();

        void NotifyUserAboutError(object sender, IFailureEventArgs e);

        void NotifyUserAboutIncomingCall(object sender, IIncomingCallEventArgs e);

        void NotifyUserAboutRejectedCall(object sender, IRejectedCallEventArgs e);
    }
}