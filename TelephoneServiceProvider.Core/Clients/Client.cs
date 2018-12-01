using TelephoneServiceProvider.Core.Contracts.Clients;

namespace TelephoneServiceProvider.Core.Clients
{
    public class Client : IClient
    {
        public IPassport Passport { get; }

        public IContract Contract { get; set; }

        public string Codeword { get; }
    }
}