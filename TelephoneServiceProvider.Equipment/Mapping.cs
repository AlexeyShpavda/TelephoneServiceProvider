﻿using TelephoneServiceProvider.Equipment.Contracts.ClientHardware.Terminal;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.BaseStation;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port;

namespace TelephoneServiceProvider.Equipment
{
    internal static class Mapping
    {
        internal static void ConnectTerminalToPort(ITerminalEvents terminal, IPortEvents port)
        {
            terminal.NotifyPortAboutOutgoingCall += port.OutgoingCall;
            port.NotifyTerminalOfFailure += terminal.NotifyUserAboutError;
            port.NotifyTerminalOfIncomingCall += terminal.NotifyUserAboutIncomingCall;
            terminal.NotifyPortAboutRejectionOfCall += port.RejectCall;
            port.NotifyTerminalOfRejectionOfCall += terminal.NotifyUserAboutRejectedCall;
            terminal.NotifyPortAboutAnsweredCall += port.AnswerCall;
        }

        internal static void ConnectPortToStation(IPortEvents port, IBaseStationEvents baseStation)
        {
            port.NotifyStationOfOutgoingCall += baseStation.NotifyIncomingCallPort;
            baseStation.NotifyPortOfFailure += port.ReportError;
            baseStation.NotifyPortOfIncomingCall += port.IncomingCall;
            port.NotifyStationOfRejectionOfCall += baseStation.RejectCall;
            baseStation.NotifyPortOfRejectionOfCall += port.InformTerminalAboutRejectionOfCall;
            port.NotifyStationOfAnsweredCall += baseStation.AnswerCall;
        }

        internal static void DisconnectTerminalFromPort(ITerminalEvents terminal, IPortEvents port)
        {
            terminal.NotifyPortAboutOutgoingCall -= port.OutgoingCall;
            port.NotifyTerminalOfFailure -= terminal.NotifyUserAboutError;
            port.NotifyTerminalOfIncomingCall -= terminal.NotifyUserAboutIncomingCall;
            terminal.NotifyPortAboutRejectionOfCall -= port.RejectCall;
            port.NotifyTerminalOfRejectionOfCall -= terminal.NotifyUserAboutRejectedCall;
            terminal.NotifyPortAboutAnsweredCall -= port.AnswerCall;
        }

        internal static void DisconnectPortFromStation(IPortEvents port, IBaseStationEvents baseStation)
        {
            port.NotifyStationOfOutgoingCall -= baseStation.NotifyIncomingCallPort;
            baseStation.NotifyPortOfFailure -= port.ReportError;
            baseStation.NotifyPortOfIncomingCall -= port.IncomingCall;
            port.NotifyStationOfRejectionOfCall -= baseStation.RejectCall;
            baseStation.NotifyPortOfRejectionOfCall -= port.InformTerminalAboutRejectionOfCall;
            port.NotifyStationOfAnsweredCall -= baseStation.AnswerCall;
        }

        internal static void MergeTerminalAndPortBehaviorWhenConnecting(ITerminalEvents terminal, IPortEvents port)
        {
            terminal.ConnectedToPort += port.ConnectToTerminal;
            terminal.DisconnectedFromPort += port.DisconnectFromTerminal;
        }

        internal static void SeparateTerminalAndPortBehaviorWhenConnecting(ITerminalEvents terminal, IPortEvents port)
        {
            terminal.ConnectedToPort -= port.ConnectToTerminal;
            terminal.DisconnectedFromPort -= port.DisconnectFromTerminal;
        }
    }
}