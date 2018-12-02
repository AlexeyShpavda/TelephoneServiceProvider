using System;
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

        public event EventHandler<ConnectionEventArgs> ConnectedToPort;

        public event EventHandler<ConnectionEventArgs> DisconnectedFromPort;

        public event EventHandler<OutgoingCallEventArgs> NotifyPortAboutOutgoingCall;

        public event EventHandler<RejectedCallEventArgs> NotifyPortAboutRejectionOfCall;

        public event EventHandler<AnsweredCallEventArgs> NotifyPortAboutAnsweredCall;

        public Guid SerialNumber { get; }

        public TerminalStatus TerminalStatus { get; private set; }

        public Terminal(Action<string> displayMethod = null)
        {
            DisplayMethod = displayMethod;
            SerialNumber = Guid.NewGuid();
            TerminalStatus = TerminalStatus.Disabled;
        }

        public void SetDisplayMethod(Action<string> action)
        {
            DisplayMethod = action;
        }

        public void ConnectToPort(IPort port)
        {
            if (IsPossibleToConnect(port))
            {
                DisplayMethod?.Invoke("ERROR! Unable to Connect to Port");
                return;
            }

            Mapping.MergeTerminalAndPortBehaviorWhenConnecting(this, port as Port);

            var connectionEventArgs = new ConnectionEventArgs(port);

            OnConnectedToPort(connectionEventArgs);

            if (connectionEventArgs.Port == null)
            {
                DisplayMethod?.Invoke("ERROR! Another Terminal is Already Connected to This Port");
                return;
            }

            Mapping.ConnectTerminalToPort(this, connectionEventArgs.Port as Port);

            TerminalStatus = TerminalStatus.Inaction;

            DisplayMethod?.Invoke("SUCCESS! Terminal is Connected");
        }

        private bool IsPossibleToConnect(IPort port)
        {
            return port != null && TerminalStatus == TerminalStatus.Disabled;
        }

        public void DisconnectFromPort()
        {
            if (TerminalStatus == TerminalStatus.Disabled)
            {
                DisplayMethod?.Invoke("ERROR! Terminal is Already Disconnected");
                return;
            }

            var connectionEventArgs = new ConnectionEventArgs(null);

            OnDisconnectedFromPort(connectionEventArgs);

            Mapping.SeparateTerminalAndPortBehaviorWhenConnecting(this, connectionEventArgs.Port as Port);

            Mapping.DisconnectTerminalFromPort(this, connectionEventArgs.Port as Port);

            TerminalStatus = TerminalStatus.Disabled;

            DisplayMethod?.Invoke("SUCCESS! Terminal is Disconnected");
        }

        public void Call(string receiverPhoneNumber)
        {
            if (TerminalStatus != TerminalStatus.Inaction) return;

            TerminalStatus = TerminalStatus.OutgoingCall;

            OnNotifyPortOfOutgoingCall(new OutgoingCallEventArgs("", receiverPhoneNumber));
        }

        public void Answer()
        {
            if (TerminalStatus != TerminalStatus.IncomingCall) return;

            TerminalStatus = TerminalStatus.Conversation;

            DisplayMethod?.Invoke("You Answered Call");

            OnNotifyPortAboutAnsweredCall(new AnsweredCallEventArgs("") { CallStartTime = DateTime.Now });
        }

        public void Reject()
        {
            if (TerminalStatus == TerminalStatus.Inaction || TerminalStatus == TerminalStatus.Disabled) return;

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

        private void OnNotifyPortOfOutgoingCall(OutgoingCallEventArgs e)
        {
            NotifyPortAboutOutgoingCall?.Invoke(this, e);
        }

        private void OnConnectedToPort(ConnectionEventArgs e)
        {
            ConnectedToPort?.Invoke(this, e);
        }

        private void OnDisconnectedFromPort(ConnectionEventArgs e)
        {
            DisconnectedFromPort?.Invoke(this, e);
        }
    }
}