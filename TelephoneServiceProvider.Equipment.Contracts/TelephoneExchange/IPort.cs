using System;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange
{
    public interface IPort
    {
        event EventHandler<IOutgoingCallEventArgs> NotifyStationOfOutgoingCall;

        event EventHandler<IRejectedCallEventArgs> NotifyStationOfRejectionOfCall;

        event EventHandler<IAnsweredCallEventArgs> NotifyStationOfAnsweredCall;

        event EventHandler<IRejectedCallEventArgs> NotifyTerminalOfRejectionOfCall;

        event EventHandler<IFailureEventArgs> NotifyTerminalOfFailure;

        event EventHandler<IIncomingCallEventArgs> NotifyTerminalOfIncomingCall;

        string PhoneNumber { get; }

        ITariff Tariff { get; }

        PortStatus PortStatus { get; }

        void ConnectToTerminal(object sender, EventArgs e);

        void DisconnectFromTerminal(object sender, EventArgs e);

        void OutgoingCall(string receiverPhoneNumber);

        void IncomingCall(object sender, IIncomingCallEventArgs e);

        void AnswerCall(object sender, IAnsweredCallEventArgs e);

        void RejectCall(object sender, IRejectedCallEventArgs e);

        void InformTerminalAboutRejectionOfCall(object sender, IRejectedCallEventArgs e);

        void ReportError(object sender, IFailureEventArgs e);

        void OnNotifyStationOfAnsweredOfCall(IAnsweredCallEventArgs e);
    }
}