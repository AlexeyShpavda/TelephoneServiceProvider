using TelephoneServiceProvider.Equipment.ClientHardware.Enums;
using TelephoneServiceProvider.Equipment.TelephoneExchange;

namespace TelephoneServiceProvider.Equipment.ClientHardware
{
    public class Terminal
    {
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
            Port = port;
            IsConnectedWithPort = true;
        }

        public void DisconnectFromPort()
        {
            Port = null;
            IsConnectedWithPort = false;
        }

        public void Call(string number)
        {
            if (TerminalStatus == TerminalStatus.SwitchedOn && IsConnectedWithPort)
            {
                Port?.Call(number);
            }
        }
    }
}