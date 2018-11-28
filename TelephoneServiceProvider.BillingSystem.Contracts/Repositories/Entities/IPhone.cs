namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities
{
    public interface IPhone
    {
        string PhoneNumber { get; set; }

        decimal Balance { get; set; }
    }

}