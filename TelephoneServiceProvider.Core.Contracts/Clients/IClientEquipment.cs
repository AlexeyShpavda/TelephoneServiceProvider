using TelephoneServiceProvider.Equipment.Contracts.ClientHardware;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port;

namespace TelephoneServiceProvider.Core.Contracts.Clients
{
    public interface IClientEquipment
    {
        ITerminal Terminal { get; }

        IPortCore Port { get; }
    }
}