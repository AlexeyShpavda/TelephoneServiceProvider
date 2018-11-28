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
            var terminal2 = client2.Contract.ClientEquipment.Terminal;

            var port1 = client1.Contract.ClientEquipment.Port;
            var port2 = client2.Contract.ClientEquipment.Port;


            terminal2.SetDisplayMethod(displayMethod);
            terminal1.SetDisplayMethod(displayMethod);


            company.BaseStation.AddPort(port1);
            company.BaseStation.AddPort(port2);


            terminal1.ConnectToPort(port1);
            terminal2.ConnectToPort(port2);


            terminal1.Call(port2.PhoneNumber);

            terminal2.Answer();

            terminal2.Reject();

            Console.WriteLine(company.Billing.GetReport(port1.PhoneNumber));

            Console.ReadKey();
        }
    }
}
