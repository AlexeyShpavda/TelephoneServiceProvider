using System;
using TelephoneServiceProvider.Equipment.Contracts.ClientHardware;
using TelephoneServiceProvider.Equipment.Contracts.ClientHardware.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;
using TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.ClientHardware
{
    public class Terminal : ITerminal
    {
        public Action<string> DisplayMethod { get; private set; }

        public event EventHandler<IRejectedCallEventArgs> NotifyPortAboutRejectionOfCall;

        public event EventHandler<IAnsweredCallEventArgs> NotifyPortAboutAnsweredCall;

        public Guid SerialNumber { get; }

        public IPort Port { get; set; }

        public TerminalStatus TerminalStatus { get; private set; }

        public Terminal()
        {
            DisplayMethod = null;
            SerialNumber = Guid.NewGuid();
            Port = null;
            TerminalStatus = TerminalStatus.Inaction;
        }

        public void SetDisplayMethod(Action<string> action)
        {
            DisplayMethod = action;
        }

        public void ConnectToPort(IPort port)
        {
            if (port == null) return;

            Port = port;
            Port.ConnectToTerminal();
            Mapping.ConnectTerminalToPort(this, Port);
        }

        public void DisconnectFromPort()
        {
            Mapping.DisconnectTerminalFromPort(this, Port);
            Port.DisconnectFromTerminal();
            Port = null;
        }

        public void Call(string receiverPhoneNumber)
        {
            if (Port == null || Port.PortStatus != PortStatus.Free ||
                TerminalStatus != TerminalStatus.Inaction) return;

            Port.OutgoingCall(receiverPhoneNumber);

            TerminalStatus = TerminalStatus.OutgoingCall;
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
                TerminalStatus != TerminalStatus.Conversation) return;

            TerminalStatus = TerminalStatus.Inaction;

            DisplayMethod?.Invoke("You Rejected Call");

            OnNotifyPortAboutRejectionOfCall(new RejectedCallEventArgs("") { CallRejectionTime = DateTime.Now });
        }

        public void NotifyUserAboutError(object sender, IFailureEventArgs e)
        {
            TerminalStatus = TerminalStatus.Inaction;

            DisplayMethod?.Invoke($"{e.ReceiverPhoneNumber} - Subscriber Doesn't Exist or He is Busy");
        }

        public void NotifyUserAboutIncomingCall(object sender, IIncomingCallEventArgs e)
        {
            TerminalStatus = TerminalStatus.IncomingCall;

            DisplayMethod?.Invoke($"{e.SenderPhoneNumber} - is calling you");
        }

        public void NotifyUserAboutRejectedCall(object sender, IRejectedCallEventArgs e)
        {
            TerminalStatus = TerminalStatus.Inaction;

            DisplayMethod?.Invoke($"{e.PhoneNumberOfPersonRejectedCall} - canceled the call");
        }

        protected virtual void OnNotifyPortAboutRejectionOfCall(IRejectedCallEventArgs e)
        {
            NotifyPortAboutRejectionOfCall?.Invoke(this, e);
        }

        protected virtual void OnNotifyPortAboutAnsweredCall(IAnsweredCallEventArgs e)
        {
            NotifyPortAboutAnsweredCall?.Invoke(this, e);
        }
    }
}