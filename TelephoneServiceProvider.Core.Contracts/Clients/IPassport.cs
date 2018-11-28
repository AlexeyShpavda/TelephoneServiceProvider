using System;

namespace TelephoneServiceProvider.Core.Contracts.Clients
{
    public interface IPassport
    {
        Guid IdentificationNumber { get; }
        string FirstName { get; }
        string LastName { get; }
    }
}