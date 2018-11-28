using System;
using TelephoneServiceProvider.BillingSystem;
using TelephoneServiceProvider.BillingSystem.Tariffs;
using TelephoneServiceProvider.Core;
using TelephoneServiceProvider.Core.Clients;
using TelephoneServiceProvider.Equipment.TelephoneExchange;

namespace TelephoneServiceProvider.PresentationLayer
{
    internal class Program
    {
        private static void Main()
        {
            Action<string> displayMethod = Console.WriteLine;

            var company = new Company("AS", new Billing(), new BaseStation());

            var tariff = new Homebody();

            var client1 = new Client();
            var client2 = new Client();

            client1.Contract = company.EnterIntoContract(client1, tariff);
            client2.Contract = company.EnterIntoContract(client2, tariff);

            var terminal1 = client1.Contract.ClientEquipment.Terminal;
            terminal1.SetDisplayMethod(displayMethod);
            var port1 = client1.Contract.ClientEquipment.Port;

            var terminal2 = client2.Contract.ClientEquipment.Terminal;
            terminal2.SetDisplayMethod(displayMethod);
            var port2 = client2.Contract.ClientEquipment.Port;

            company.BaseStation.AddPort(port1);
            company.BaseStation.AddPort(port2);

            terminal1.ConnectedToPort += port1.ConnectToTerminal;
            terminal1.DisconnectedFromPort += port1.DisconnectFromTerminal;

            terminal1.ConnectToPort(port1);

            port1.NotifyStationOfOutgoingCall += company.BaseStation.NotifyIncomingCallPort;
            company.BaseStation.NotifyPortOfFailure += port1.ReportError;
            port1.NotifyTerminalOfFailure += terminal1.NotifyUserAboutError;
            company.BaseStation.NotifyPortOfIncomingCall += port1.IncomingCall;
            port1.NotifyTerminalOfIncomingCall += terminal1.NotifyUserAboutIncomingCall;

            terminal1.NotifyPortAboutRejectionOfCall += port1.RejectCall;
            port1.NotifyStationOfRejectionOfCall += company.BaseStation.RejectCall;
            company.BaseStation.NotifyPortOfRejectionOfCall += port1.InformTerminalAboutRejectionOfCall;
            port1.NotifyTerminalOfRejectionOfCall += terminal1.NotifyUserAboutRejectedCall;

            terminal1.NotifyPortAboutAnsweredCall += port1.AnswerCall;
            port1.NotifyStationOfAnsweredCall += company.BaseStation.AnswerCall;

            terminal2.ConnectedToPort += port2.ConnectToTerminal;
            terminal2.DisconnectedFromPort += port2.DisconnectFromTerminal;

            terminal2.ConnectToPort(port2);

            port2.NotifyStationOfOutgoingCall += company.BaseStation.NotifyIncomingCallPort;
            company.BaseStation.NotifyPortOfFailure += port2.ReportError;
            port2.NotifyTerminalOfFailure += terminal2.NotifyUserAboutError;
            company.BaseStation.NotifyPortOfIncomingCall += port2.IncomingCall;
            port2.NotifyTerminalOfIncomingCall += terminal2.NotifyUserAboutIncomingCall;

            terminal2.NotifyPortAboutRejectionOfCall += port2.RejectCall;
            port2.NotifyStationOfRejectionOfCall += company.BaseStation.RejectCall;
            company.BaseStation.NotifyPortOfRejectionOfCall += port2.InformTerminalAboutRejectionOfCall;
            port2.NotifyTerminalOfRejectionOfCall += terminal2.NotifyUserAboutRejectedCall;

            terminal2.NotifyPortAboutAnsweredCall += port2.AnswerCall;
            port2.NotifyStationOfAnsweredCall += company.BaseStation.AnswerCall;


            terminal1.Call(port2.PhoneNumber);

            terminal2.Answer();

            terminal2.Reject();

            Console.WriteLine(company.Billing.GetReport(port1.PhoneNumber));

            Console.ReadKey();
        }
    }
}
