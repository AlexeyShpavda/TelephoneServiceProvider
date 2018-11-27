namespace TelephoneServiceProvider.Core.Clients
{
    public class Client
    {
        public Passport Passport;

        public Contract Contract { get; set; }

        public string Codeword { get; private set; }
    }
}