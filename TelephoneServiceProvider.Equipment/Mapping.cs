using TelephoneServiceProvider.Equipment.Contracts.ClientHardware;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange;

namespace TelephoneServiceProvider.Equipment
{
    public static class Mapping
    {
        public static void ConnectTerminalToPort(ITerminal terminal, IPort port)
        {
            port.NotifyTerminalOfFailure += terminal.NotifyUserAboutError;
            port.NotifyTerminalOfIncomingCall += terminal.NotifyUserAboutIncomingCall;
            terminal.NotifyPortAboutRejectionOfCall += port.RejectCall;
            port.NotifyTerminalOfRejectionOfCall += terminal.NotifyUserAboutRejectedCall;
            terminal.NotifyPortAboutAnsweredCall += port.AnswerCall;
        }

        public static void ConnectPortToStation(IPort port, IBaseStation baseStation)
        {
            port.NotifyStationOfOutgoingCall += baseStation.NotifyIncomingCallPort;
            baseStation.NotifyPortOfFailure += port.ReportError;
            baseStation.NotifyPortOfIncomingCall += port.IncomingCall;
            port.NotifyStationOfRejectionOfCall += baseStation.RejectCall;
            baseStation.NotifyPortOfRejectionOfCall += port.InformTerminalAboutRejectionOfCall;
            port.NotifyStationOfAnsweredCall += baseStation.AnswerCall;
        }

        public static void DisconnectTerminalFromPort(ITerminal terminal, IPort port)
        {
            port.NotifyTerminalOfFailure -= terminal.NotifyUserAboutError;
            port.NotifyTerminalOfIncomingCall -= terminal.NotifyUserAboutIncomingCall;
            terminal.NotifyPortAboutRejectionOfCall -= port.RejectCall;
            port.NotifyTerminalOfRejectionOfCall -= terminal.NotifyUserAboutRejectedCall;
            terminal.NotifyPortAboutAnsweredCall -= port.AnswerCall;
        }

        public static void DisconnectPortFromStation(IPort port, IBaseStation baseStation)
        {
            port.NotifyStationOfOutgoingCall -= baseStation.NotifyIncomingCallPort;
            baseStation.NotifyPortOfFailure -= port.ReportError;
            baseStation.NotifyPortOfIncomingCall -= port.IncomingCall;
            port.NotifyStationOfRejectionOfCall -= baseStation.RejectCall;
            baseStation.NotifyPortOfRejectionOfCall -= port.InformTerminalAboutRejectionOfCall;
            port.NotifyStationOfAnsweredCall -= baseStation.AnswerCall;
        }
    }
}