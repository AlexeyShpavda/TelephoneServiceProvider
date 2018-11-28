using System;
using TelephoneServiceProvider.Equipment.TelephoneExchange;
using TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.ClientHardware
{
    public class Terminal
    {
        public Action<string> DisplayMethod { get; private set; }

        public event EventHandler ConnectedToPort;

        public event EventHandler DisconnectedFromPort;

        public event EventHandler<RejectedCallEventArgs> NotifyPortAboutRejectionOfCall;

        public event EventHandler<AnsweredCallEventArgs> NotifyPortAboutAnsweredCall;

        public Guid SerialNumber { get; }

        public bool IsConnectedWithPort { get; private set; }

        private Port Port { get; set; }

        public Terminal()
        {
            DisplayMethod = null;
            SerialNumber = Guid.NewGuid();
            IsConnectedWithPort = false;
            Port = null;

            //Mapping.SubscribeToSyncWithTerminal(this, Port);
        }

        public void SetDisplayMethod(Action<string> action)
        {
            DisplayMethod = action;
        }

        public void ConnectToPort(Port port)
        {
            if (port == null) return;

            Port = port;
            IsConnectedWithPort = true;
            OnConnectedToPort();
        }

        public void DisconnectFromPort()
        {
            Port = null;
            IsConnectedWithPort = false;
            OnDisconnectedFromPort();
        }

        public void Call(string receiverPhoneNumber)
        {
            if (IsConnectedWithPort)
            {
                Port.OutgoingCall(receiverPhoneNumber);
            }
        }

        public void Answer()
        {
            DisplayMethod?.Invoke("You Answered Call");

            OnNotifyPortAboutAnsweredCall(new AnsweredCallEventArgs("") {CallStartTime = DateTime.Now});
        }

        public void Reject()
        {
            DisplayMethod?.Invoke("You Rejected Call");

            OnNotifyPortAboutRejectionOfCall(new RejectedCallEventArgs("") {CallRejectionTime = DateTime.Now});
        }

        public void NotifyUserAboutError(object sender, FailureEventArgs e)
        {
            DisplayMethod?.Invoke($"{e.ReceiverPhoneNumber} - Subscriber Doesn't Exist or He is Busy");
        }

        public void NotifyUserAboutIncomingCall(object sender, IncomingCallEventArgs e)
        {
            DisplayMethod?.Invoke($"{e.SenderPhoneNumber} - is calling you");
        }

        public void NotifyUserAboutRejectedCall(object sender, RejectedCallEventArgs e)
        {
            DisplayMethod?.Invoke($"{e.PhoneNumberOfPersonRejectedCall} - canceled the call");
        }

        protected virtual void OnConnectedToPort()
        {
            ConnectedToPort?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDisconnectedFromPort()
        {
            DisconnectedFromPort?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnNotifyPortAboutRejectionOfCall(RejectedCallEventArgs e)
        {
            NotifyPortAboutRejectionOfCall?.Invoke(this, e);
        }

        protected virtual void OnNotifyPortAboutAnsweredCall(AnsweredCallEventArgs e)
        {
            NotifyPortAboutAnsweredCall?.Invoke(this, e);
        }
    }
}