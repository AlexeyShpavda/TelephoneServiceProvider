using System;
using TelephoneServiceProvider.Equipment.TelephoneExchange;
using TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.ClientHardware
{
    public class Terminal
    {
        public event EventHandler ConnectedToPort;

        public event EventHandler DisconnectedFromPort;

        public event EventHandler<RejectedCallEventArgs> NotifyPortAboutRejectionOfCall;

        public bool IsConnectedWithPort { get; private set; }

        public bool IsThereUnansweredCall;

        private Port Port { get; set; }

        public Terminal()
        {
            IsConnectedWithPort = false;
            IsThereUnansweredCall = false;
            Port = null;
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

        public void Reject()
        {
            if (IsThereUnansweredCall)
            {
                Console.WriteLine("You Rejected Call");
                IsThereUnansweredCall = false;
                OnNotifyPortAboutRejectionOfCall(new RejectedCallEventArgs(""));
            }
            else
            {
                Console.WriteLine("Nobody Calls You");
            }

        }

        public void NotifyUserAboutError(object sender, FailureEventArguments e)
        {
            Console.WriteLine($"{e.ReceiverPhoneNumber} - Subscriber Doesn't Exist or He is Busy");
        }

        public void NotifyUserAboutIncomingCall(object sender, IncomingCallEventArguments e)
        {
            IsThereUnansweredCall = true;
            Console.WriteLine($"{e.SenderPhoneNumber} - is calling you");
        }

        public void NotifyUserAboutRejectedCall(object sender, RejectedCallEventArgs e)
        {
            Console.WriteLine($"{e.PhoneNumberOfPersonRejectedCall} - canceled the call");
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
    }
}