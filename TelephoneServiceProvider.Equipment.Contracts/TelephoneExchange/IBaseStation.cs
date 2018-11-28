using System;
using System.Collections.Generic;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange
{
    public interface IBaseStation
    {
        event EventHandler<IIncomingCallEventArgs> NotifyPortOfIncomingCall;

        event EventHandler<IRejectedCallEventArgs> NotifyPortOfRejectionOfCall;

        event EventHandler<IFailureEventArgs> NotifyPortOfFailure;

        event EventHandler<ICall> NotifyBillingSystemAboutCallEnd;

        IList<IPort> Ports { get; }

        IDictionary<IPort, IPort> CallsWaitingToBeAnswered { get; }

        IList<ICall> CallsInProgress { get; }

        void NotifyIncomingCallPort(object sender, IOutgoingCallEventArgs e);

        void AnswerCall(object sender, IAnsweredCallEventArgs e);

        void RejectCall(object sender, IRejectedCallEventArgs e);

        void AddPort(IPort port);
    }
}