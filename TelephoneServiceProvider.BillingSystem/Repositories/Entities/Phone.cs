using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Repositories.Entities
{
    public class Phone : IPhone
    {
        public string PhoneNumber { get; set; }

        public decimal Balance { get; set; }

        public Phone(string phoneNumber)
        {
            Balance = 0;
            PhoneNumber = phoneNumber;
        }
    }
}