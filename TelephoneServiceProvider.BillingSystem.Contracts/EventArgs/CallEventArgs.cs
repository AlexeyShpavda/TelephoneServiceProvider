using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Contracts.EventArgs
{
    public class CallEventArgs : System.EventArgs, ICall
    {
        public string SenderPhoneNumber { get; }

        public string ReceiverPhoneNumber { get; }

        protected CallEventArgs(string senderPhoneNumber, string receiverPhoneNumber)
        {
            SenderPhoneNumber = senderPhoneNumber;

            ReceiverPhoneNumber = receiverPhoneNumber;
        }
    }
}