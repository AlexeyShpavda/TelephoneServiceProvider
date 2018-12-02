using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public class ConnectionEventArgs
    {
        public IPortCore Port { get; set; }

        public ConnectionEventArgs(IPortCore port)
        {
            Port = port;
        }
    }
}