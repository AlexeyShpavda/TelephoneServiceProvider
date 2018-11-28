namespace TelephoneServiceProvider.Core.Contracts.Clients
{
    public interface IClient
    {
        IPassport Passport { get; set; }

        IContract Contract { get; set; }

        string Codeword { get; }
    }
}