using System;
using System.Collections.Generic;
using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange
{
    public interface IBaseStation
    {
        int CancellationTime { get; }

        event EventHandler<IncomingCallEventArgs> NotifyPortOfIncomingCall;

        event EventHandler<RejectedCallEventArgs> NotifyPortOfRejectionOfCall;

        event EventHandler<FailureEventArgs> NotifyPortOfFailure;

        event EventHandler<CallEventArgs> NotifyBillingSystemAboutCallEnd;

        event EventHandler<CheckBalanceEventArgs> CheckBalanceInBillingSystem;

        IList<IPortCore> Ports { get; }

        IDictionary<IPortCore, IPortCore> CallsWaitingToBeAnswered { get; }

        IList<ICall> CallsInProgress { get; }

        void AddPorts(IEnumerable<IPortCore> ports);

        void RemovePorts(IEnumerable<IPortCore> ports);

        void AddPort(IPortCore port);

        void RemovePort(IPortCore port);
    }
}