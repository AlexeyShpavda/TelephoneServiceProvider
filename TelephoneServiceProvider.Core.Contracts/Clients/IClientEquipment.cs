using TelephoneServiceProvider.Equipment.Contracts.ClientHardware;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange;

namespace TelephoneServiceProvider.Core.Contracts.Clients
{
    public interface IClientEquipment
    {
        ITerminal Terminal { get; }

        IPort Port { get; }
    }
}