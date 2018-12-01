using System;
using System.Collections.Generic;
using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange
{
    public interface IBaseStation
    {
        event EventHandler<IncomingCallEventArgs> NotifyPortOfIncomingCall;

        event EventHandler<RejectedCallEventArgs> NotifyPortOfRejectionOfCall;

        event EventHandler<FailureEventArgs> NotifyPortOfFailure;

        event EventHandler<CallEventArgs> NotifyBillingSystemAboutCallEnd;

        event EventHandler<CheckBalanceEventArgs> CheckBalanceInBillingSystem;

        IList<IPort> Ports { get; }

        IDictionary<IPort, IPort> CallsWaitingToBeAnswered { get; }

        IList<ICall> CallsInProgress { get; }

        void AddPorts(IEnumerable<IPort> ports);

        void RemovePorts(IEnumerable<IPort> ports);

        void AddPort(IPort port);

        void RemovePort(IPort port);
    }
}