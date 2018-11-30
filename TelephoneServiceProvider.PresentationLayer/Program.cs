using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TelephoneServiceProvider.BillingSystem;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;
using TelephoneServiceProvider.BillingSystem.Tariffs;
using TelephoneServiceProvider.Core;
using TelephoneServiceProvider.Core.Clients;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange;
using TelephoneServiceProvider.Equipment.TelephoneExchange;

namespace TelephoneServiceProvider.PresentationLayer
{
    internal class Program
    {
        private static void Main()
        {
            Action<string> displayMethod = Console.WriteLine;

            var company = new Company("AS", new Billing(new List<ITariff> {new Homebody()}), new BaseStation());

            var tariff = company.Billing.Tariffs.First();

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

            company.BaseStation.AddPorts(new List<IPort> {port1, port2});

            terminal1.ConnectToPort(port1);
            terminal2.ConnectToPort(port2);

            company.Billing.IncreaseBalance(port1.PhoneNumber, 10);
            company.Billing.IncreaseBalance(port2.PhoneNumber, 10);

            terminal1.Call(port2.PhoneNumber);

            terminal2.Answer();
            Thread.Sleep(5000);
            terminal2.Reject();

            Console.WriteLine($"Balance at 1 terminal: {company.Billing.GetBalance(terminal1.Port.PhoneNumber)}");
            Console.WriteLine($"Balance at 2 terminal: {company.Billing.GetBalance(terminal2.Port.PhoneNumber)}");

            Console.WriteLine(company.Billing.GetReport(port1.PhoneNumber));

            Console.ReadKey();
        }
    }
}
