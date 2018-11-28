using System.Linq;
using TelephoneServiceProvider.BillingSystem.Repositories.Entities;
using TelephoneServiceProvider.Core.Contracts.EventArgs;

namespace TelephoneServiceProvider.BillingSystem
{
    public class Billing
    {
        private BillingUnitOfWork Data { get; set; }

        public Billing()
        {
            Data = new BillingUnitOfWork();
        }

        public void PutCallOnRecord(object sender, Call e)
        {
            Data.Calls.Add(new Call(e.SenderPhoneNumber, e.ReceiverPhoneNumber, e.CallStartTime, e.CallEndTime));
        }

        public void PutPhoneOnRecord(object sender, IBillingSystemEventArgs e)
        {
            var newPhone = new Phone(e.PhoneNumber);
            Data.Phones.Add(newPhone);
        }

        public decimal GetBalance(string phoneNumber)
        {
            var phone = GetPhoneOnNumber(phoneNumber);

            return phone?.Balance ?? default(decimal);
        }

        public void RechargeBalance(string phoneNumber, decimal amountOfMoney)
        {
            var phone = GetPhoneOnNumber(phoneNumber);

            if (phone != null)
            {
                phone.Balance += amountOfMoney;
            }
        }

        public Phone GetPhoneOnNumber(string phoneNumber)
        {
            return Data.Phones.GetAll().FirstOrDefault(x => x.PhoneNumber == phoneNumber);
        }
    }
}