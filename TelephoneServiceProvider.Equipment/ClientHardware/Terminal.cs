using System;
using TelephoneServiceProvider.Equipment.ClientHardware.Enums;
using TelephoneServiceProvider.Equipment.TelephoneExchange;
using TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.ClientHardware
{
    public class Terminal
    {
        public event EventHandler ConnectedToPort;

        public event EventHandler DisconnectedFromPort;

        public TerminalStatus TerminalStatus { get; private set; }

        public bool IsConnectedWithPort { get; private set; }

        private Port Port { get; set; }

        public Terminal()
        {
            TerminalStatus = TerminalStatus.SwitchedOff;
            IsConnectedWithPort = false;
            Port = null;
        }

        public void SwitchOn()
        {
            if (TerminalStatus == TerminalStatus.SwitchedOff)
            {
                TerminalStatus = TerminalStatus.SwitchedOn;
            }
        }

        public void SwitchOff()
        {
            if (TerminalStatus == TerminalStatus.SwitchedOn)
            {
                TerminalStatus = TerminalStatus.SwitchedOff;
            }
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
            if (TerminalStatus == TerminalStatus.SwitchedOn && IsConnectedWithPort)
            {
                Port.OutgoingCall(receiverPhoneNumber);
            }
        }

        public void NotifyUserAboutError(object sender, FailureEventArguments e)
        {
            Console.WriteLine($"{e.ReceiverPhoneNumber} - Subscriber Doesn't Exist or He is Busy");
        }

        public void NotifyUserAboutIncomingCall(object sender, IncomingCallEventArguments e)
        {
            Console.WriteLine($"{e.SenderPhoneNumber} - is calling you");
        }

        protected virtual void OnConnectedToPort()
        {
            ConnectedToPort?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDisconnectedFromPort()
        {
            DisconnectedFromPort?.Invoke(this, EventArgs.Empty);
        }
    }
}