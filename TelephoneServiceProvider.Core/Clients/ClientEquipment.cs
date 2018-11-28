using TelephoneServiceProvider.Core.Contracts.Clients;

namespace TelephoneServiceProvider.Core.Clients
{
    public class ClientEquipment : IClientEquipment
    {
        public ITerminal Terminal { get; private set; }

        public IPort Port { get; private set; }

        public ClientEquipment(ITerminal terminal, IPort port)
        {
            Terminal = terminal;
            Port = port;
        }
    }
}