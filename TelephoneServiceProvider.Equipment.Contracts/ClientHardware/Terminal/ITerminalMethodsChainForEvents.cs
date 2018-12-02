using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.Contracts.ClientHardware.Terminal
{
    public interface ITerminalMethodsChainForEvents
    {
        void NotifyUserAboutError(object sender, FailureEventArgs e);

        void NotifyUserAboutIncomingCall(object sender, IncomingCallEventArgs e);

        void NotifyUserAboutRejectedCall(object sender, RejectedCallEventArgs e);
    }
}