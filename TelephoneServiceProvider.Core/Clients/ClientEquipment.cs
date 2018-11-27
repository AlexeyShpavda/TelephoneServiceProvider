using TelephoneServiceProvider.Equipment.ClientHardware;
using TelephoneServiceProvider.Equipment.TelephoneExchange;

namespace TelephoneServiceProvider.Core.Clients
{
    public class ClientEquipment
    {
        public Terminal Terminal { get; private set; }

        public Port Port { get; private set; }

        public ClientEquipment(Terminal terminal, Port port)
        {
            Terminal = terminal;
            Port = port;
        }
    }
}