using TelephoneServiceProvider.Core.Contracts.Clients;
using TelephoneServiceProvider.Equipment.Contracts.ClientHardware;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange;

namespace TelephoneServiceProvider.Core.Clients
{
    public class ClientEquipment : IClientEquipment
    {
        public ITerminal Terminal { get; }

        public IPort Port { get; }

        public ClientEquipment(ITerminal terminal, IPort port)
        {
            Terminal = terminal;
            Port = port;
        }
    }
}