namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public class ConnectionEventArgs
    {
        public IPort Port { get; set; }

        public ConnectionEventArgs(IPort port)
        {
            Port = port;
        }
    }
}