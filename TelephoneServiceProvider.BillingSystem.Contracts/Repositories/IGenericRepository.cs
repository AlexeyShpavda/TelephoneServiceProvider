using System.Collections.Generic;

namespace TelephoneServiceProvider.BillingSystem.Contracts.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        ICollection<T> GetAll();
        void Add(T entity);
        void Remove(T entity);
    }
}