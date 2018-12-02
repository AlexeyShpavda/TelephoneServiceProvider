//using System;
//using TelephoneServiceProvider.Equipment.Contracts.ClientHardware.Enums;
//using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange;

//namespace TelephoneServiceProvider.Equipment.Contracts.ClientHardware.Terminal
//{
//    internal interface ITerminalCoreFunctionality
//    {
//        Guid SerialNumber { get; }

//        TerminalStatus TerminalStatus { get; }

//        void SetDisplayMethod(Action<string> action);

//        void ConnectToPort(IPort port);

//        void DisconnectFromPort();

//        void Call(string receiverPhoneNumber);

//        void Answer();

//        void Reject();
//    }
//}