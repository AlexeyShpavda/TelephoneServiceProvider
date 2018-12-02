using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.BillingSystem.Contracts.Tariffs.Abstract;

namespace TelephoneServiceProvider.BillingSystem.Contracts
{
    public interface IPhoneManagement
    {
        IPhone GetPhoneOnNumber(string phoneNumber);

        void PutPhoneOnRecord(string phoneNumber, ITariff tariff);
    }
}