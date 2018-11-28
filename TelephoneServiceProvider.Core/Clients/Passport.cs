using System;
using TelephoneServiceProvider.Core.Contracts.Clients;

namespace TelephoneServiceProvider.Core.Clients
{
    public class Passport : IPassport
    {
        public Guid IdentificationNumber { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public Passport(string firstName, string lastName)
        {
            IdentificationNumber = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
        }
    }
}