using System;
using System.Collections.Generic;
using System.Linq;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.BillingSystem.Repositories.Entities;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;
using TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange
{
    public class BaseStation : IBaseStation
    {
        public event EventHandler<IIncomingCallEventArgs> NotifyPortOfIncomingCall;

        public event EventHandler<IRejectedCallEventArgs> NotifyPortOfRejectionOfCall;

        public event EventHandler<IFailureEventArgs> NotifyPortOfFailure;

        public event EventHandler<ICall> NotifyBillingSystemAboutCallEnd;

        public IList<IPort> Ports { get; }

        public IDictionary<IPort, IPort> CallsWaitingToBeAnswered { get; private set; }

        public IList<ICall> CallsInProgress { get; private set; }

        public BaseStation()
        {
            CallsWaitingToBeAnswered = new Dictionary<IPort, IPort>();
            CallsInProgress = new List<ICall>();
            Ports = new List<IPort>();
        }

        public BaseStation(IList<IPort> ports) : this()
        {
            Ports = ports;
        }

        public void NotifyIncomingCallPort(object sender, IOutgoingCallEventArgs e)
        {
            var senderPort = (IPort)sender;
            var receiverPort = Ports.FirstOrDefault(x => x.PhoneNumber == e.ReceiverPhoneNumber);

            if (receiverPort != null && receiverPort.PortStatus == PortStatus.Free)
            {
                CallsWaitingToBeAnswered.Add(senderPort, receiverPort);

                OnNotifyPortOfIncomingCall(new IncomingCallEventArgs(senderPort.PhoneNumber), senderPort,
                    receiverPort);
            }
            else
            {
                OnNotifyPortOfFailure(new FailureEventArgs(e.ReceiverPhoneNumber), senderPort);
            }
        }

        public void AnswerCall(object sender, IAnsweredCallEventArgs e)
        {
            var receiverPort = (IPort)sender;
            var senderPort = CallsWaitingToBeAnswered.FirstOrDefault(x => x.Value == receiverPort).Key;

            if (senderPort == null) return;

            CallsWaitingToBeAnswered.Remove(senderPort);

            CallsInProgress.Add(new AnsweredCall(senderPort.PhoneNumber, receiverPort.PhoneNumber)
            { CallStartTime = e.CallStartTime });
        }

        public void RejectCall(object sender, IRejectedCallEventArgs e)
        {
            var portRejectedCall = (IPort)sender;

            var portWhichNeedToSendNotification = CallsInProgress.FirstOrDefault(x =>
                x.ReceiverPhoneNumber == portRejectedCall.PhoneNumber ||
                x.SenderPhoneNumber == portRejectedCall.PhoneNumber) is IAnsweredCall canceledCall
                ? CompleteCallInProgress(portRejectedCall, canceledCall, e)
                : CancelNotStartedCall(portRejectedCall);

            OnNotifyPortAboutRejectionOfCall(e, portWhichNeedToSendNotification);
        }

        public void AddPort(IPort port)
        {
            Mapping.LinkPortAndStation(port, this);

            Ports.Add(port);
        }

        private IPort CompleteCallInProgress(IPort portRejectedCall, IAnsweredCall canceledCall, IRejectedCallEventArgs e)
        {
            var portWhichNeedToSendNotification = canceledCall.SenderPhoneNumber == portRejectedCall.PhoneNumber
                ? Ports.FirstOrDefault(x => x.PhoneNumber == canceledCall.ReceiverPhoneNumber)
                : Ports.FirstOrDefault(x => x.PhoneNumber == canceledCall.SenderPhoneNumber);

            CallsInProgress.Remove(canceledCall);

            canceledCall.CallEndTime = e.CallRejectionTime;

            OnNotifyBillingSystemAboutCallEnd(canceledCall);

            return portWhichNeedToSendNotification;
        }

        private IPort CancelNotStartedCall(IPort portRejectedCall)
        {
            IPort portWhichNeedToSendNotification;

            if (CallsWaitingToBeAnswered.ContainsKey(portRejectedCall))
            {
                portWhichNeedToSendNotification = CallsWaitingToBeAnswered[portRejectedCall];

                CallsWaitingToBeAnswered.Remove(portRejectedCall);
            }
            else
            {
                portWhichNeedToSendNotification =
                    CallsWaitingToBeAnswered.FirstOrDefault(x => x.Value == portRejectedCall).Key;

                if (portWhichNeedToSendNotification != null)
                    CallsWaitingToBeAnswered.Remove(portWhichNeedToSendNotification);
            }

            return portWhichNeedToSendNotification;
        }

        protected virtual void OnNotifyPortOfIncomingCall(IIncomingCallEventArgs e, IPort senderPort, IPort receiverPort)
        {
            try
            {
                (NotifyPortOfIncomingCall?.GetInvocationList().First(x => x.Target == receiverPort) as
                    EventHandler<IIncomingCallEventArgs>)?.Invoke(this, e);
            }
            catch (Exception)
            {
                OnNotifyPortOfFailure(new FailureEventArgs(receiverPort.PhoneNumber), senderPort);
            }
        }

        protected virtual void OnNotifyPortOfFailure(IFailureEventArgs e, IPort port)
        {
            (NotifyPortOfFailure?.GetInvocationList().First(x => x.Target == port) as
                EventHandler<IFailureEventArgs>)?.Invoke(this, e);
        }

        protected virtual void OnNotifyPortAboutRejectionOfCall(IRejectedCallEventArgs e, IPort port)
        {
            (NotifyPortOfRejectionOfCall?.GetInvocationList().First(x => x.Target == port) as
                EventHandler<IRejectedCallEventArgs>)?.Invoke(this, e);
        }

        protected virtual void OnNotifyBillingSystemAboutCallEnd(ICall e)
        {
            NotifyBillingSystemAboutCallEnd?.Invoke(this, e);
        }
    }
}