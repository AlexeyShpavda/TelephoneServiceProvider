﻿using System;
using TelephoneServiceProvider.Equipment.Contracts.ClientHardware;
using TelephoneServiceProvider.Equipment.Contracts.ClientHardware.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;
using TelephoneServiceProvider.Equipment.TelephoneExchange;

namespace TelephoneServiceProvider.Equipment.ClientHardware
{
    public class Terminal : ITerminal
    {
        public Action<string> DisplayMethod { get; private set; }

        public event EventHandler<RejectedCallEventArgs> NotifyPortAboutRejectionOfCall;

        public event EventHandler<AnsweredCallEventArgs> NotifyPortAboutAnsweredCall;

        public Guid SerialNumber { get; }

        public IPort Port { get; private set; }

        public TerminalStatus TerminalStatus { get; private set; }

        public Terminal(IPort port = null, Action<string> displayMethod = null)
        {
            DisplayMethod = displayMethod;
            SerialNumber = Guid.NewGuid();
            Port = port;
            TerminalStatus = TerminalStatus.Inaction;
        }

        public void SetDisplayMethod(Action<string> action)
        {
            DisplayMethod = action;
        }

        public void ConnectToPort(IPort port)
        {
            if (port == null || port.PortStatus != PortStatus.SwitchedOff) return;

            Port = port;
            Port.ConnectToTerminal();
            Mapping.ConnectTerminalToPort(this, Port as Port);
        }

        public void DisconnectFromPort()
        {
            Mapping.DisconnectTerminalFromPort(this, Port as Port);
            Port.DisconnectFromTerminal();
            Port = null;
        }

        public void Call(string receiverPhoneNumber)
        {
            if (Port == null || Port.PortStatus != PortStatus.Free ||
                TerminalStatus != TerminalStatus.Inaction) return;

            TerminalStatus = TerminalStatus.OutgoingCall;

            Port.OutgoingCall(receiverPhoneNumber);
        }

        public void Answer()
        {
            if (Port == null || Port.PortStatus != PortStatus.Busy ||
                TerminalStatus != TerminalStatus.IncomingCall) return;

            TerminalStatus = TerminalStatus.Conversation;

            DisplayMethod?.Invoke("You Answered Call");

            OnNotifyPortAboutAnsweredCall(new AnsweredCallEventArgs("") { CallStartTime = DateTime.Now });
        }

        public void Reject()
        {
            if (Port == null || Port.PortStatus != PortStatus.Busy ||
                TerminalStatus == TerminalStatus.Inaction) return;

            TerminalStatus = TerminalStatus.Inaction;

            DisplayMethod?.Invoke("You Rejected Call");

            OnNotifyPortAboutRejectionOfCall(new RejectedCallEventArgs("") { CallRejectionTime = DateTime.Now });
        }

        internal void NotifyUserAboutError(object sender, FailureEventArgs e)
        {
            TerminalStatus = TerminalStatus.Inaction;

            switch (e.FailureType)
            {
                case FailureType.InsufficientFunds:
                    DisplayMethod?.Invoke("You don't have enough funds to make a call");
                    break;
                case FailureType.SubscriberIsBusy:
                    DisplayMethod?.Invoke($"{e.ReceiverPhoneNumber} - Subscriber is Busy");
                    break;
                case FailureType.SubscriberDoesNotExist:
                    DisplayMethod?.Invoke($"{e.ReceiverPhoneNumber} - Subscriber Doesn't Exist");
                    break;
                default:
                    DisplayMethod?.Invoke("Unknown error");
                    break;
            }
        }

        internal void NotifyUserAboutIncomingCall(object sender, IncomingCallEventArgs e)
        {
            TerminalStatus = TerminalStatus.IncomingCall;

            DisplayMethod?.Invoke($"{e.SenderPhoneNumber} - is calling you");
        }

        internal void NotifyUserAboutRejectedCall(object sender, RejectedCallEventArgs e)
        {
            TerminalStatus = TerminalStatus.Inaction;

            DisplayMethod?.Invoke($"{e.PhoneNumberOfPersonRejectedCall} - canceled the call");
        }

        private void OnNotifyPortAboutRejectionOfCall(RejectedCallEventArgs e)
        {
            NotifyPortAboutRejectionOfCall?.Invoke(this, e);
        }

        private void OnNotifyPortAboutAnsweredCall(AnsweredCallEventArgs e)
        {
            NotifyPortAboutAnsweredCall?.Invoke(this, e);
        }
    }
}