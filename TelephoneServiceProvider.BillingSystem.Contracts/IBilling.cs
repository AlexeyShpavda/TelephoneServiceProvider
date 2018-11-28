using System;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;

namespace TelephoneServiceProvider.BillingSystem.Contracts
{
    public interface IBilling
    {
        decimal GetBalance(string phoneNumber);

        void RechargeBalance(string phoneNumber, decimal amountOfMoney);

        string GetReport(string phoneNumber, Func<ICall, bool> selector = null);
    }
}