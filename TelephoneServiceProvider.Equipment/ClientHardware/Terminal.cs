using TelephoneServiceProvider.Equipment.ClientHardware.Enums;
using TelephoneServiceProvider.Equipment.TelephoneExchange;

namespace TelephoneServiceProvider.Equipment.ClientHardware
{
    public class Terminal
    {
        public TerminalStatus TerminalStatus { get; private set; }

        private Port Port { get; set; }

        public Terminal(Port port)
        {
            Port = port;
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

        public void Call(string number)
        {
            Port.Call(number);
        }
    }
}