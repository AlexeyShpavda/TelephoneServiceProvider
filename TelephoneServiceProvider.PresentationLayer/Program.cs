using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TelephoneServiceProvider.BillingSystem;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;
using TelephoneServiceProvider.BillingSystem.Tariffs;
using TelephoneServiceProvider.Core;
using TelephoneServiceProvider.Core.Clients;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port;
using TelephoneServiceProvider.Equipment.TelephoneExchange;

namespace TelephoneServiceProvider.PresentationLayer
{
    internal class Program
    {
        private static void Main()
        {
            Action<string> displayMethod = Console.WriteLine;

            var company = new Company("AS", new Billing(new List<ITariff> { new Homebody() }), new BaseStation());

            var tariff = company.Billing.Tariffs.First();

            var client1 = new Client(new Passport("Edmond", "D"));
            var client2 = new Client(new Passport("Tom", "S"));
            var client3 = new Client(new Passport("Gary", "G"));

            client1.Contract = company.EnterIntoContract(client1, tariff);
            client2.Contract = company.EnterIntoContract(client2, tariff);
            client3.Contract = company.EnterIntoContract(client3, tariff);

            var terminal1 = client1.Contract.ClientEquipment.Terminal;
            var terminal2 = client2.Contract.ClientEquipment.Terminal;
            var terminal3 = client3.Contract.ClientEquipment.Terminal;

            var port1 = client1.Contract.ClientEquipment.Port;
            var port2 = client2.Contract.ClientEquipment.Port;
            var port3 = client3.Contract.ClientEquipment.Port;

            terminal2.SetDisplayMethod(displayMethod);
            terminal1.SetDisplayMethod(displayMethod);
            terminal3.SetDisplayMethod(displayMethod);

            company.BaseStation.AddPorts(new List<IPortCore> { port1, port2, port3 });

            terminal1.ConnectToPort(port1);
            terminal2.ConnectToPort(port2);
            terminal3.ConnectToPort(port3);

            terminal1.Call(port2.PhoneNumber);

            terminal3.Call(port2.PhoneNumber);

            Thread.Sleep(5000);
            terminal2.Answer();
            Thread.Sleep(3000);
            terminal2.Reject();

            terminal3.Call("123");

            Console.WriteLine("Balance at 1 terminal after call: " +
                              $"{company.Billing.BalanceOperation.GetBalance(client1.Contract.PhoneNumber)}");
            Console.WriteLine("Balance at 2 terminal after call: " +
                              $"{company.Billing.BalanceOperation.GetBalance(client2.Contract.PhoneNumber)}");

            terminal1.Call(port2.PhoneNumber);

            company.Billing.BalanceOperation.IncreaseBalance(client1.Contract.PhoneNumber, 10);
            company.Billing.BalanceOperation.IncreaseBalance(client2.Contract.PhoneNumber, 10);

            terminal1.Call(port2.PhoneNumber);

            terminal2.Answer();
            Thread.Sleep(3000);
            terminal2.Reject();

            Console.WriteLine(company.Billing.GetCallReport(port1.PhoneNumber));

            Console.ReadKey();
        }
    }
}