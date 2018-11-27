using System;
using TelephoneServiceProvider.Equipment.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange
{
    public class Port
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

        public void ConnectToTerminal(object sender, EventArgs e)
        {
            PortStatus = PortStatus.Free;
        }

        public void DisconnectFromTerminal(object sender, EventArgs e)
        {
            PortStatus = PortStatus.SwitchedOff;
        }

        public void OutgoingCall(string receiverPhoneNumber)
        {
            if (PortStatus != PortStatus.Free || PhoneNumber == receiverPhoneNumber) return;

            PortStatus = PortStatus.Busy;
            OnNotifyStationOfOutgoingCall(new OutgoingCallEventArgs(PhoneNumber, receiverPhoneNumber));
        }

        public void IncomingCall(object sender, IncomingCallEventArgs e)
        {
            PortStatus = PortStatus.Busy;

            OnNotifyTerminalOfIncomingCall(e);
        }

        public void AnswerCall(object sender, AnsweredCallEventArgs e)
        {
            OnNotifyStationOfAnsweredOfCall(new AnsweredCallEventArgs(PhoneNumber) {CallStartTime = e.CallStartTime});
        }

        public void RejectCall(object sender, RejectedCallEventArgs e)
        {
            PortStatus = PortStatus.Free;

            OnNotifyStationAboutRejectionOfCall(new RejectedCallEventArgs(PhoneNumber));
        }

        public void InformTerminalAboutRejectionOfCall(object sender, RejectedCallEventArgs e)
        {
            PortStatus = PortStatus.Free;

            OnNotifyTerminalOfRejectionOfCall(e);
        }

        public void ReportError(object sender, FailureEventArgs e)
        {
            PortStatus = PortStatus.Free;

            OnNotifyTerminalOfFailure(e);
        }

        protected virtual void OnNotifyStationOfOutgoingCall(OutgoingCallEventArgs e)
        {
            NotifyStationOfOutgoingCall?.Invoke(this, e);
        }

        protected virtual void OnNotifyTerminalOfFailure(FailureEventArgs e)
        {
            NotifyTerminalOfFailure?.Invoke(this, e);
        }

        protected virtual void OnNotifyTerminalOfIncomingCall(IncomingCallEventArgs e)
        {
            NotifyTerminalOfIncomingCall?.Invoke(this, e);
        }

        protected virtual void OnNotifyStationAboutRejectionOfCall(RejectedCallEventArgs e)
        {
            NotifyStationOfRejectionOfCall?.Invoke(this, e);
        }

        protected virtual void OnNotifyTerminalOfRejectionOfCall(RejectedCallEventArgs e)
        {
            NotifyTerminalOfRejectionOfCall?.Invoke(this, e);
        }

        protected virtual void OnNotifyStationOfAnsweredOfCall(AnsweredCallEventArgs e)
        {
            NotifyStationOfAnsweredCall?.Invoke(this, e);
        }
    }
}