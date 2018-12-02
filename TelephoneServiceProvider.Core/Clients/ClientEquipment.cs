using TelephoneServiceProvider.Core.Contracts.Clients;
using TelephoneServiceProvider.Equipment.Contracts.ClientHardware;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port;

namespace TelephoneServiceProvider.Core.Clients
{
    public class ClientEquipment : IClientEquipment
    {
        public ITerminal Terminal { get; }

        public IPortCore Port { get; }

        public ClientEquipment(ITerminal terminal, IPortCore port)
        {
            Terminal = terminal;
            Port = port;
        }
    }
}