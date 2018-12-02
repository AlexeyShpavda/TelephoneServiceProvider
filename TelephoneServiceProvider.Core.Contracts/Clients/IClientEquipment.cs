using TelephoneServiceProvider.Equipment.Contracts.ClientHardware.Terminal;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port;

namespace TelephoneServiceProvider.Core.Contracts.Clients
{
    public interface IClientEquipment
    {
        ITerminalCore Terminal { get; }

        IPortCore Port { get; }
    }
}