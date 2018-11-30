namespace TelephoneServiceProvider.BillingSystem.Contracts.EventArgs
{
    public interface ICheckBalanceEventArgs
    {
        string PhoneNumber { get; set; }

        bool IsAllowedCall { get; set; }
    }
}