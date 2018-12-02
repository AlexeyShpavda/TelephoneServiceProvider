using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.BaseStation
{
    public interface IBaseStationMethodsChainForEvents
    {
        void NotifyIncomingCallPort(object sender, OutgoingCallEventArgs e);

        void AnswerCall(object sender, AnsweredCallEventArgs e);

        void RejectCall(object sender, RejectedCallEventArgs e);
    }
}