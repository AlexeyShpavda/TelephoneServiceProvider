using System;
using System.Linq;
using System.Text;
using TelephoneServiceProvider.BillingSystem.Contracts;
using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.BillingSystem.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem
{
    public class Billing : IBilling
    {
        private IBillingUnitOfWork Data { get; set; }

        public Billing()
        {
            Data = new BillingUnitOfWork();
        }

        public void PutCallOnRecord(object sender, ICall e)
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

        public string GetReport(string phoneNumber, Func<ICall, bool> selector = null)
        {
            var report = new StringBuilder();

            var subscriberCalls = selector != null
                ? Data.Calls.GetAll()
                    .Where(x => x.SenderPhoneNumber == phoneNumber || x.ReceiverPhoneNumber == phoneNumber)
                    .Where(selector).ToList()
                : Data.Calls.GetAll()
                    .Where(x => x.SenderPhoneNumber == phoneNumber || x.ReceiverPhoneNumber == phoneNumber)
                    .ToList();

            foreach (var item in subscriberCalls)
            {
                report.Append(item + "\n");
            }

            return report.ToString();
        }

        public IPhone GetPhoneOnNumber(string phoneNumber)
        {
            return Data.Phones.GetAll().FirstOrDefault(x => x.PhoneNumber == phoneNumber);
        }
    }
}