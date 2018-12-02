using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port
{
    public interface IPortCore
    {
        string PhoneNumber { get; }

        PortStatus PortStatus { get; }

        void ConnectToTerminal(object sender, ConnectionEventArgs e);

        void DisconnectFromTerminal(object sender, ConnectionEventArgs e);

        void OutgoingCall(object sender, OutgoingCallEventArgs e);
    }
}