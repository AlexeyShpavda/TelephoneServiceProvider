using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port
{
    public interface IPortMethodsChainForEvents
    {
        void ConnectToTerminal(object sender, ConnectionEventArgs e);

        void DisconnectFromTerminal(object sender, ConnectionEventArgs e);

        void OutgoingCall(object sender, OutgoingCallEventArgs e);

        void IncomingCall(object sender, IncomingCallEventArgs e);

        void AnswerCall(object sender, AnsweredCallEventArgs e);

        void RejectCall(object sender, RejectedCallEventArgs e);

        void InformTerminalAboutRejectionOfCall(object sender, RejectedCallEventArgs e);

        void ReportError(object sender, FailureEventArgs e);
    }
}