namespace TelephoneServiceProvider.BillingSystem.Contracts.EventArgs
{
    public class CheckBalanceEventArgs
    {
        public string PhoneNumber { get; set; }

        public bool IsAllowedCall { get; set; }

        public CheckBalanceEventArgs(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
    }
}