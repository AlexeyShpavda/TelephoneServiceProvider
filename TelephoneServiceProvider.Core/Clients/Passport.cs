using System;
using TelephoneServiceProvider.Core.Contracts.Clients;

namespace TelephoneServiceProvider.Core.Clients
{
    public class Passport : IPassport
    {
        public Guid IdentificationNumber { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public Passport(string firstName, string lastName)
        {
            IdentificationNumber = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
        }
    }
}