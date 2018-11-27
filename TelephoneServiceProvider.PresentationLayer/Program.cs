using System;
using System.Collections.Generic;
using TelephoneServiceProvider.Equipment.ClientHardware;
using TelephoneServiceProvider.Equipment.TelephoneExchange;

namespace TelephoneServiceProvider.PresentationLayer
{
    internal class Program
    {
        private static void Main()
        {
            var port1 = new Port("1");
            var terminal1 = new Terminal();

            var port2 = new Port("2");
            var terminal2 = new Terminal();

            var baseStation = new BaseStation(new List<Port> {port1, port2});

            terminal1.ConnectedToPort += port1.ConnectToTerminal;
            terminal1.DisconnectedFromPort += port1.DisconnectFromTerminal;

            terminal1.ConnectToPort(port1);

            port1.NotifyStationOfOutgoingCall += baseStation.NotifyIncomingCallPort;
            baseStation.NotifyPortOfFailure += port1.ReportError;
            port1.NotifyTerminalOfFailure += terminal1.NotifyUserAboutError;
            baseStation.NotifyPortOfIncomingCall += port1.IncomingCall;
            port1.NotifyTerminalOfIncomingCall += terminal1.NotifyUserAboutIncomingCall;

            terminal1.NotifyPortAboutRejectionOfCall += port1.RejectCall;
            port1.NotifyStationOfRejectionOfCall += baseStation.RejectCall;
            baseStation.NotifyPortOfRejectionOfCall += port1.InformTerminalAboutRejectionOfCall;
            port1.NotifyTerminalOfRejectionOfCall += terminal1.NotifyUserAboutRejectedCall;

            terminal1.NotifyPortAboutAnsweredCall += port1.AnswerCall;
            port1.NotifyStationOfAnsweredCall += baseStation.AnswerCall;

            terminal2.ConnectedToPort += port2.ConnectToTerminal;
            terminal2.DisconnectedFromPort += port2.DisconnectFromTerminal;

            terminal2.ConnectToPort(port2);

            port2.NotifyStationOfOutgoingCall += baseStation.NotifyIncomingCallPort;
            baseStation.NotifyPortOfFailure += port2.ReportError;
            port2.NotifyTerminalOfFailure += terminal2.NotifyUserAboutError;
            baseStation.NotifyPortOfIncomingCall += port2.IncomingCall;
            port2.NotifyTerminalOfIncomingCall += terminal2.NotifyUserAboutIncomingCall;

            terminal2.NotifyPortAboutRejectionOfCall += port2.RejectCall;
            port2.NotifyStationOfRejectionOfCall += baseStation.RejectCall;
            baseStation.NotifyPortOfRejectionOfCall += port2.InformTerminalAboutRejectionOfCall;
            port2.NotifyTerminalOfRejectionOfCall += terminal2.NotifyUserAboutRejectedCall;

            terminal2.NotifyPortAboutAnsweredCall += port2.AnswerCall;
            port2.NotifyStationOfAnsweredCall += baseStation.AnswerCall;

            terminal1.Call("2");

            terminal2.Answer();

            Console.ReadKey();
        }
    }
}
