using System;
using System.Collections.Generic;
using System.Linq;
using TelephoneServiceProvider.BillingSystem.Tariffs.Abstract;
using TelephoneServiceProvider.Core.Clients;
using TelephoneServiceProvider.Equipment.ClientHardware;
using TelephoneServiceProvider.Equipment.TelephoneExchange;

namespace TelephoneServiceProvider.Core
{
    public class Company
    {
        public string Name { get; private set; }

        public ICollection<Client> Clients { get; set; }

        public ICollection<Contract> Contracts { get; set; }

        public Company(string name)
        {
            Name = name;
            Clients = new List<Client>();
            Contracts = new List<Contract>();
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
    }
}