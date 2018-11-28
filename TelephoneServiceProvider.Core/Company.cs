using System;
using System.Collections.Generic;
using System.Linq;
using TelephoneServiceProvider.BillingSystem;
using TelephoneServiceProvider.BillingSystem.Tariffs.Abstract;
using TelephoneServiceProvider.Core.Clients;
using TelephoneServiceProvider.Core.Contracts.EventArgs;
using TelephoneServiceProvider.Core.EventArgs;
using TelephoneServiceProvider.Equipment.ClientHardware;
using TelephoneServiceProvider.Equipment.TelephoneExchange;

namespace TelephoneServiceProvider.Core
{
    public class Company
    {
        public event EventHandler<IBillingSystemEventArgs> ReportBillingSystemOfNewClient;

        public string Name { get; private set; }

        public ICollection<Client> Clients { get; private set; }

        public ICollection<Contract> Contracts { get; private set; }

        public Billing Billing { get; private set; }

        public BaseStation BaseStation { get; private set; }

        public Company(string name, Billing billing, BaseStation baseStation)
        {
            Name = name;
            Clients = new List<Client>();
            Contracts = new List<Contract>();
            Billing = billing;
            BaseStation = baseStation;

            SubscribeToEvents();
        }

        public Contract EnterIntoContract(Client client, Tariff selectedTariff)
        {
            var company = this;
            var passport = client.Passport;
            var phoneNumber = GetUniquePhoneNumber();
            var tariff = selectedTariff;
            var clientEquipment = new ClientEquipment(new Terminal(), new Port(phoneNumber, selectedTariff));
            var conditions = "Do not break equipment";

            var newContract = new Contract(company, passport, phoneNumber, tariff, clientEquipment, conditions);

            Contracts.Add(newContract);

            if (Clients.All(x => x != client))
            {
                Clients.Add(client);
            }

            OnReportBillingSystemOfNewClient(new BillingSystemEventArgs(phoneNumber));

            return newContract;
        }

        private string GetUniquePhoneNumber()
        {
            var random = new Random();

            string generatedPhoneNumber;

            do
            {
                generatedPhoneNumber = Convert.ToString(random.Next());
            } while (Contracts.Select(x => x.ClientEquipment.Port.PhoneNumber).Contains(generatedPhoneNumber));

            return generatedPhoneNumber;
        }

        private void SubscribeToEvents()
        {
            ReportBillingSystemOfNewClient += Billing.PutPhoneOnRecord;
            BaseStation.NotifyBillingSystemAboutCallEnd += Billing.PutCallOnRecord;
        }

        protected virtual void OnReportBillingSystemOfNewClient(BillingSystemEventArgs e)
        {
            ReportBillingSystemOfNewClient?.Invoke(this, e);
        }
    }
}