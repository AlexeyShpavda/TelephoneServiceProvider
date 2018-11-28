using TelephoneServiceProvider.BillingSystem.Contracts;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.BillingSystem.Repositories;

namespace TelephoneServiceProvider.BillingSystem
{
    public class BillingUnitOfWork : IBillingUnitOfWork
    {
        public IGenericRepository<IPhone> Phones { get; }

        public IGenericRepository<ICall> Calls { get; }

        public BillingUnitOfWork()
        {
            Phones = new GenericRepository<IPhone>();
            
            Calls = new GenericRepository<ICall>();
        }
    }
}