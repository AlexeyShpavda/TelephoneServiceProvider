using System;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange
{
    public class Port : IPort
    {
        public event EventHandler<OutgoingCallEventArgs> NotifyStationOfOutgoingCall;

        public event EventHandler<RejectedCallEventArgs> NotifyStationOfRejectionOfCall;

        public event EventHandler<AnsweredCallEventArgs> NotifyStationOfAnsweredCall;

        public event EventHandler<RejectedCallEventArgs> NotifyTerminalOfRejectionOfCall;

        public event EventHandler<FailureEventArgs> NotifyTerminalOfFailure;

        public event EventHandler<IncomingCallEventArgs> NotifyTerminalOfIncomingCall;

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

        internal void IncomingCall(object sender, IncomingCallEventArgs e)
        {
            PortStatus = PortStatus.Busy;

            OnNotifyTerminalOfIncomingCall(e);
        }

        internal void AnswerCall(object sender, AnsweredCallEventArgs e)
        {
            OnNotifyStationOfAnsweredOfCall(new AnsweredCallEventArgs(PhoneNumber) { CallStartTime = e.CallStartTime });
        }

        internal void RejectCall(object sender, RejectedCallEventArgs e)
        {
            PortStatus = PortStatus.Free;

            OnNotifyStationAboutRejectionOfCall(new RejectedCallEventArgs(PhoneNumber)
            { CallRejectionTime = e.CallRejectionTime });
        }

        internal void InformTerminalAboutRejectionOfCall(object sender, RejectedCallEventArgs e)
        {
            PortStatus = PortStatus.Free;

            OnNotifyTerminalOfRejectionOfCall(e);
        }

        internal void ReportError(object sender, FailureEventArgs e)
        {
            PortStatus = PortStatus.Free;

            OnNotifyTerminalOfFailure(e);
        }

        private void OnNotifyStationOfOutgoingCall(OutgoingCallEventArgs e)
        {
            NotifyStationOfOutgoingCall?.Invoke(this, e);
        }

        private void OnNotifyTerminalOfFailure(FailureEventArgs e)
        {
            NotifyTerminalOfFailure?.Invoke(this, e);
        }

        private void OnNotifyTerminalOfIncomingCall(IncomingCallEventArgs e)
        {
            NotifyTerminalOfIncomingCall?.Invoke(this, e);
        }

        private void OnNotifyStationAboutRejectionOfCall(RejectedCallEventArgs e)
        {
            NotifyStationOfRejectionOfCall?.Invoke(this, e);
        }

        private void OnNotifyTerminalOfRejectionOfCall(RejectedCallEventArgs e)
        {
            NotifyTerminalOfRejectionOfCall?.Invoke(this, e);
        }

        private void OnNotifyStationOfAnsweredOfCall(AnsweredCallEventArgs e)
        {
            NotifyStationOfAnsweredCall?.Invoke(this, e);
        }
    }
}