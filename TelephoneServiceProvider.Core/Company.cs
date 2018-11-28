using System;
using System.Collections.Generic;
using System.Linq;
using TelephoneServiceProvider.BillingSystem.Contracts;
using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;
using TelephoneServiceProvider.Core.Clients;
using TelephoneServiceProvider.Core.Contracts;
using TelephoneServiceProvider.Core.Contracts.Clients;
using TelephoneServiceProvider.Core.EventArgs;
using TelephoneServiceProvider.Equipment.ClientHardware;
using TelephoneServiceProvider.Equipment.TelephoneExchange;

namespace TelephoneServiceProvider.Core
{
    public class Company : ICompany
    {
        public event EventHandler<IBillingSystemEventArgs> ReportBillingSystemOfNewClient;

        public string Name { get; private set; }

        public ICollection<IClient> Clients { get; private set; }

        public ICollection<IContract> Contracts { get; private set; }

        public IBilling Billing { get; private set; }

        public IBaseStation BaseStation { get; private set; }

        public Company(string name, IBilling billing, IBaseStation baseStation)
        {
            Name = name;
            Clients = new List<IClient>();
            Contracts = new List<IContract>();
            Billing = billing;
            BaseStation = baseStation;

            SubscribeToEvents();
        }

        public IContract EnterIntoContract(IClient client, ITariff selectedTariff)
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