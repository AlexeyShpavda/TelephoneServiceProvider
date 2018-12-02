using TelephoneServiceProvider.Core.Contracts.Clients;
using TelephoneServiceProvider.Equipment.Contracts.ClientHardware.Terminal;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port;

namespace TelephoneServiceProvider.Core.Clients
{
    public class ClientEquipment : IClientEquipment
    {
        public ITerminalCore Terminal { get; }

        public IPortCore Port { get; }

        public ClientEquipment(ITerminalCore terminal, IPortCore port)
        {
            Terminal = terminal;
            Port = port;
        }
    }
}