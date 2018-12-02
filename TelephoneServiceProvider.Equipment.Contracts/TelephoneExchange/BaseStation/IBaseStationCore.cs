using System.Collections.Generic;
using System.Timers;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.BaseStation
{
    public interface IBaseStationCore
    {
        int CancellationTime { get; }

        IList<IPortCore> Ports { get; }

        IDictionary<IPortCore, IPortCore> CallsWaitingToBeAnswered { get; }

        IDictionary<IPortCore, Timer> PortTimeout { get; }

        IList<ICall> CallsInProgress { get; }

        void AddPorts(IEnumerable<IPortCore> ports);

        void RemovePorts(IEnumerable<IPortCore> ports);

        void AddPort(IPortCore port);

        void RemovePort(IPortCore port);
    }
}