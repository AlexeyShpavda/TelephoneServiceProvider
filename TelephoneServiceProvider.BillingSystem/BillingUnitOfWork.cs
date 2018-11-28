using TelephoneServiceProvider.BillingSystem.Repositories;
using TelephoneServiceProvider.BillingSystem.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem
{
    public class BillingUnitOfWork
    {
        public GenericRepository<Phone> Phones { get; }

        public GenericRepository<Call> Calls { get; }

        public BillingUnitOfWork()
        {
            Phones = new GenericRepository<Phone>();
            
            Calls = new GenericRepository<Call>();
        }
    }
}