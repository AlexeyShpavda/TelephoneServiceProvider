using System.Collections.Generic;
using TelephoneServiceProvider.Core.Clients;

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
    }
}