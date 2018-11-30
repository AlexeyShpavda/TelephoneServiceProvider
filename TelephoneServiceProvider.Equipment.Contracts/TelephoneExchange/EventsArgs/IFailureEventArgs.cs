using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs
{
    public interface IFailureEventArgs
    {
        string ReceiverPhoneNumber { get; set; }

        FailureType FailureType { get; set; }
    }
}