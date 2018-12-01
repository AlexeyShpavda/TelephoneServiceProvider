using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Contracts
{
    public interface IPhoneManagement
    {
        IPhone GetPhoneOnNumber(string phoneNumber);
    }
}