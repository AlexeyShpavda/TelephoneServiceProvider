namespace TelephoneServiceProvider.Core.Contracts.Clients
{
    public interface IClient
    {
        IPassport Passport { get; }

        IContract Contract { get; set; }

        string Codeword { get; set; }
    }
}