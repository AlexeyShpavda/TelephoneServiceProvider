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

        public IList<IPort> Ports { get; private set; }

        public IDictionary<IPort, IPort> CallsWaitingToBeAnswered { get; private set; }

        public IList<ICall> CallsInProgress { get; private set; }

        public BaseStation()
        {
            CallsWaitingToBeAnswered = new Dictionary<IPort, IPort>();
            CallsInProgress = new List<ICall>();
            Ports = new List<IPort>();
        }

        public BaseStation(IEnumerable<IPort> ports) : this()
        {
            AddPorts(ports);
        }

        public void AddPorts(IEnumerable<IPort> ports)
        {
            foreach (var port in ports)
            {
                AddPort(port);
            }
        }

        public void RemovePorts(IEnumerable<IPort> ports)
        {
            foreach (var port in ports)
            {
                RemovePort(port);
            }
        }

        public void AddPort(IPort port)
        {
            Mapping.ConnectPortToStation(port as Port, this);

            Ports.Add(port);
        }

        public void RemovePort(IPort port)
        {
            Mapping.DisconnectPortFromStation(port as Port, this);

            Ports.Remove(port);
        }

        internal void NotifyIncomingCallPort(object sender, IOutgoingCallEventArgs e)
        {
            var senderPort = sender as IPort;

            var receiverPort = Ports.FirstOrDefault(x => x.PhoneNumber == e.ReceiverPhoneNumber);

            if (receiverPort != null && senderPort != null && receiverPort.PortStatus == PortStatus.Free)
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

        internal void AnswerCall(object sender, IAnsweredCallEventArgs e)
        {
            var receiverPort = sender as IPort;

            var senderPort = CallsWaitingToBeAnswered.FirstOrDefault(x => x.Value == receiverPort).Key;

            if (senderPort == null) return;

            CallsWaitingToBeAnswered.Remove(senderPort);

            if (receiverPort != null)
            {
                CallsInProgress.Add(new AnsweredCall(senderPort.PhoneNumber, receiverPort.PhoneNumber)
                    {CallStartTime = e.CallStartTime});
            }
        }

        internal void RejectCall(object sender, IRejectedCallEventArgs e)
        {
            var portRejectedCall = sender as IPort;

            var suitableCall = CallsInProgress.FirstOrDefault(x =>
                portRejectedCall != null && (x.ReceiverPhoneNumber == portRejectedCall.PhoneNumber ||
                                             x.SenderPhoneNumber == portRejectedCall.PhoneNumber));

            var portWhichNeedToSendNotification = suitableCall is IAnsweredCall answeredCall
                ? CompleteCallInProgress(portRejectedCall, answeredCall, e)
                : CancelNotStartedCall(portRejectedCall, e);

            OnNotifyPortAboutRejectionOfCall(e, portWhichNeedToSendNotification);
        }

        private IPort CompleteCallInProgress(IPort portRejectedCall, IAnsweredCall call, IRejectedCallEventArgs e)
        {
            var portWhichNeedToSendNotification = call.SenderPhoneNumber == portRejectedCall.PhoneNumber
                ? Ports.FirstOrDefault(x => x.PhoneNumber == call.ReceiverPhoneNumber)
                : Ports.FirstOrDefault(x => x.PhoneNumber == call.SenderPhoneNumber);

            CallsInProgress.Remove(call);

            OnNotifyBillingSystemAboutCallEnd(new AnsweredCall(call.SenderPhoneNumber, call.ReceiverPhoneNumber,
                call.CallStartTime, e.CallRejectionTime));

            return portWhichNeedToSendNotification;
        }

        private IPort CancelNotStartedCall(IPort portRejectedCall, IRejectedCallEventArgs e)
        {
            IPort portWhichNeedToSendNotification;
            string senderPhoneNumber;
            string receiverPhoneNumber;

            if (CallsWaitingToBeAnswered.ContainsKey(portRejectedCall))
            {
                portWhichNeedToSendNotification = CallsWaitingToBeAnswered[portRejectedCall];

                senderPhoneNumber = portRejectedCall.PhoneNumber;
                receiverPhoneNumber = portWhichNeedToSendNotification.PhoneNumber;

                CallsWaitingToBeAnswered.Remove(portRejectedCall);
            }
            else
            {
                portWhichNeedToSendNotification =
                    CallsWaitingToBeAnswered.FirstOrDefault(x => x.Value == portRejectedCall).Key;

                senderPhoneNumber = portWhichNeedToSendNotification.PhoneNumber;
                receiverPhoneNumber = portRejectedCall.PhoneNumber;

                CallsWaitingToBeAnswered.Remove(portWhichNeedToSendNotification);
            }

            OnNotifyBillingSystemAboutCallEnd(new UnansweredCall(senderPhoneNumber, receiverPhoneNumber,
                e.CallRejectionTime));

            return portWhichNeedToSendNotification;
        }

        private void OnNotifyPortOfIncomingCall(IIncomingCallEventArgs e, IPort senderPort, IPort receiverPort)
        {
            if (NotifyPortOfIncomingCall?.GetInvocationList().FirstOrDefault(x => x.Target == receiverPort) != null)
            {
                (NotifyPortOfIncomingCall?.GetInvocationList().FirstOrDefault(x => x.Target == receiverPort) as
                    EventHandler<IIncomingCallEventArgs>)?.Invoke(this, e);
            }
            else
            {
                OnNotifyPortOfFailure(new FailureEventArgs(receiverPort.PhoneNumber), senderPort);
            }
        }

        private void OnNotifyPortOfFailure(IFailureEventArgs e, IPort port)
        {
            if (NotifyPortOfFailure?.GetInvocationList().FirstOrDefault(x => x.Target == port) != null)
            {
                (NotifyPortOfFailure?.GetInvocationList().First(x => x.Target == port) as
                    EventHandler<IFailureEventArgs>)?.Invoke(this, e);
            }
        }

        private void OnNotifyPortAboutRejectionOfCall(IRejectedCallEventArgs e, IPort port)
        {
            if (NotifyPortOfRejectionOfCall?.GetInvocationList().FirstOrDefault(x => x.Target == port) != null)
            {
                (NotifyPortOfRejectionOfCall?.GetInvocationList().First(x => x.Target == port) as
                    EventHandler<IRejectedCallEventArgs>)?.Invoke(this, e);
            }
        }

        private void OnNotifyBillingSystemAboutCallEnd(ICall e)
        {
            NotifyBillingSystemAboutCallEnd?.Invoke(this, e);
        }
    }
}