using System;
using TelephoneServiceProvider.Equipment.Contracts.ClientHardware.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port;

namespace TelephoneServiceProvider.Equipment.Contracts.ClientHardware.Terminal
{
    public interface ITerminalCore
    {
        Action<string> DisplayMethod { get; }

        Guid SerialNumber { get; }

        TerminalStatus TerminalStatus { get; }

        void SetDisplayMethod(Action<string> action);

        void ConnectToPort(IPortCore port);

        void DisconnectFromPort();

        void Call(string receiverPhoneNumber);

        void Answer();

        void Reject();
    }
}