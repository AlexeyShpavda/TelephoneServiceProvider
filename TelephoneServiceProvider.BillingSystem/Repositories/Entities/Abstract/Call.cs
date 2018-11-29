using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Repositories.Entities.Abstract
{
    public abstract class Call : ICall
    {
        public string SenderPhoneNumber { get; }

        public string ReceiverPhoneNumber { get; }

        protected Call(string senderPhoneNumber, string receiverPhoneNumber)
        {
            SenderPhoneNumber = senderPhoneNumber;

            ReceiverPhoneNumber = receiverPhoneNumber;
        }

        public override string ToString()
        {
            return $"{SenderPhoneNumber} | {ReceiverPhoneNumber}";
        }
    }
}