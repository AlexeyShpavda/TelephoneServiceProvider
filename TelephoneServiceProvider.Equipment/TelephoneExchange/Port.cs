﻿using System;
using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;
using TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange
{
    public class Port : IPort
    {
        public event EventHandler<IOutgoingCallEventArgs> NotifyStationOfOutgoingCall;

        public event EventHandler<IRejectedCallEventArgs> NotifyStationOfRejectionOfCall;

        public event EventHandler<IAnsweredCallEventArgs> NotifyStationOfAnsweredCall;

        public event EventHandler<IRejectedCallEventArgs> NotifyTerminalOfRejectionOfCall;

        public event EventHandler<IFailureEventArgs> NotifyTerminalOfFailure;

        public event EventHandler<IIncomingCallEventArgs> NotifyTerminalOfIncomingCall;

        public event EventHandler<ICheckBalanceEventArgs> NotifyTerminalOfLackOfMoneyInAccount;

        public string PhoneNumber { get; private set; }

        public PortStatus PortStatus { get; private set; }

        public Port(string phoneNumber)
        {
            PortStatus = PortStatus.SwitchedOff;
            PhoneNumber = phoneNumber;
        }

        public void ConnectToTerminal()
        {
            PortStatus = PortStatus.Free;
        }

        public void DisconnectFromTerminal()
        {
            PortStatus = PortStatus.SwitchedOff;
        }

        public void OutgoingCall(string receiverPhoneNumber)
        {
            if (PortStatus != PortStatus.Free || PhoneNumber == receiverPhoneNumber) return;

            PortStatus = PortStatus.Busy;
            OnNotifyStationOfOutgoingCall(new OutgoingCallEventArgs(PhoneNumber, receiverPhoneNumber));
        }

        internal void IncomingCall(object sender, IIncomingCallEventArgs e)
        {
            PortStatus = PortStatus.Busy;

            OnNotifyTerminalOfIncomingCall(e);
        }

        internal void AnswerCall(object sender, IAnsweredCallEventArgs e)
        {
            OnNotifyStationOfAnsweredOfCall(new AnsweredCallEventArgs(PhoneNumber) { CallStartTime = e.CallStartTime });
        }

        internal void RejectCall(object sender, IRejectedCallEventArgs e)
        {
            PortStatus = PortStatus.Free;

            OnNotifyStationAboutRejectionOfCall(new RejectedCallEventArgs(PhoneNumber)
            { CallRejectionTime = e.CallRejectionTime });
        }

        internal void InformTerminalAboutRejectionOfCall(object sender, IRejectedCallEventArgs e)
        {
            PortStatus = PortStatus.Free;

            OnNotifyTerminalOfRejectionOfCall(e);
        }

        internal void InformTerminalAboutLackOfMoneyInAccount(object sender, ICheckBalanceEventArgs e)
        {
            PortStatus = PortStatus.Free;

            OnNotifyTerminalOfLackOfMoneyInAccount(e);
        }

        internal void ReportError(object sender, IFailureEventArgs e)
        {
            PortStatus = PortStatus.Free;

            OnNotifyTerminalOfFailure(e);
        }

        private void OnNotifyStationOfOutgoingCall(IOutgoingCallEventArgs e)
        {
            NotifyStationOfOutgoingCall?.Invoke(this, e);
        }

        private void OnNotifyTerminalOfFailure(IFailureEventArgs e)
        {
            NotifyTerminalOfFailure?.Invoke(this, e);
        }

        private void OnNotifyTerminalOfIncomingCall(IIncomingCallEventArgs e)
        {
            NotifyTerminalOfIncomingCall?.Invoke(this, e);
        }

        private void OnNotifyStationAboutRejectionOfCall(IRejectedCallEventArgs e)
        {
            NotifyStationOfRejectionOfCall?.Invoke(this, e);
        }

        private void OnNotifyTerminalOfRejectionOfCall(IRejectedCallEventArgs e)
        {
            NotifyTerminalOfRejectionOfCall?.Invoke(this, e);
        }

        private void OnNotifyStationOfAnsweredOfCall(IAnsweredCallEventArgs e)
        {
            NotifyStationOfAnsweredCall?.Invoke(this, e);
        }

        protected virtual void OnNotifyTerminalOfLackOfMoneyInAccount(ICheckBalanceEventArgs e)
        {
            NotifyTerminalOfLackOfMoneyInAccount?.Invoke(this, e);
        }
    }
}