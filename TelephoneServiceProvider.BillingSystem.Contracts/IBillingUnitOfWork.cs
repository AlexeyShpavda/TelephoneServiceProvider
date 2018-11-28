using TelephoneServiceProvider.BillingSystem.Contracts.Repositories;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Contracts
{
    public interface IBillingUnitOfWork
    {
        IGenericRepository<IPhone> Phones { get; }

        IGenericRepository<ICall> Calls { get; }
    }
}