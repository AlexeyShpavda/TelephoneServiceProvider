using System;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;
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

        public string PhoneNumber { get; private set; }

        public ITariff Tariff { get; private set; }

        public PortStatus PortStatus { get; private set; }

        public Port(string phoneNumber, ITariff tariff)
        {
            PortStatus = PortStatus.SwitchedOff;
            PhoneNumber = phoneNumber;
            Tariff = tariff;
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

        public void IncomingCall(object sender, IIncomingCallEventArgs e)
        {
            PortStatus = PortStatus.Busy;

            OnNotifyTerminalOfIncomingCall(e);
        }

        public void AnswerCall(object sender, IAnsweredCallEventArgs e)
        {
            OnNotifyStationOfAnsweredOfCall(new AnsweredCallEventArgs(PhoneNumber) { CallStartTime = e.CallStartTime });
        }

        public void RejectCall(object sender, IRejectedCallEventArgs e)
        {
            PortStatus = PortStatus.Free;

            OnNotifyStationAboutRejectionOfCall(new RejectedCallEventArgs(PhoneNumber)
            { CallRejectionTime = e.CallRejectionTime });
        }

        public void InformTerminalAboutRejectionOfCall(object sender, IRejectedCallEventArgs e)
        {
            PortStatus = PortStatus.Free;

            OnNotifyTerminalOfRejectionOfCall(e);
        }

        public void ReportError(object sender, IFailureEventArgs e)
        {
            PortStatus = PortStatus.Free;

            OnNotifyTerminalOfFailure(e);
        }

        protected virtual void OnNotifyStationOfOutgoingCall(IOutgoingCallEventArgs e)
        {
            NotifyStationOfOutgoingCall?.Invoke(this, e);
        }

        protected virtual void OnNotifyTerminalOfFailure(IFailureEventArgs e)
        {
            NotifyTerminalOfFailure?.Invoke(this, e);
        }

        protected virtual void OnNotifyTerminalOfIncomingCall(IIncomingCallEventArgs e)
        {
            NotifyTerminalOfIncomingCall?.Invoke(this, e);
        }

        protected virtual void OnNotifyStationAboutRejectionOfCall(IRejectedCallEventArgs e)
        {
            NotifyStationOfRejectionOfCall?.Invoke(this, e);
        }

        protected virtual void OnNotifyTerminalOfRejectionOfCall(IRejectedCallEventArgs e)
        {
            NotifyTerminalOfRejectionOfCall?.Invoke(this, e);
        }

        public virtual void OnNotifyStationOfAnsweredOfCall(IAnsweredCallEventArgs e)
        {
            NotifyStationOfAnsweredCall?.Invoke(this, e);
        }
    }
}