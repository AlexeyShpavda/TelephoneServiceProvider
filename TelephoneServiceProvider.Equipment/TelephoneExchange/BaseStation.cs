using System;
using System.Collections.Generic;
using System.Linq;
using TelephoneServiceProvider.BillingSystem;
using TelephoneServiceProvider.Equipment.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange
{
    public class BaseStation
    {
        public event EventHandler<IncomingCallEventArguments> NotifyPortOfIncomingCall;

        public event EventHandler<RejectedCallEventArgs> NotifyPortOfRejectionOfCall;

        public event EventHandler<FailureEventArguments> NotifyPortOfFailure;

        public IList<Port> Ports { get; }

        public IDictionary<Port, Port> CallsWaitingToBeAnswered { get; private set; }

        public ICollection<Call> CallsInProgress { get; private set; }

        public BaseStation(IList<Port> ports)
        {
            CallsWaitingToBeAnswered = new Dictionary<Port, Port>();
            CallsInProgress = new List<Call>();
            Ports = ports;
        }

        public void NotifyIncomingCallPort(object sender, OutgoingCallEventArgs e)
        {
            var senderPort = (Port) sender;
            var receiverPort = Ports.FirstOrDefault(x => x.PhoneNumber == e.ReceiverPhoneNumber);

            if (receiverPort != null && receiverPort.PortStatus == PortStatus.Free)
            {
                CallsWaitingToBeAnswered.Add(senderPort, receiverPort);

                OnNotifyPortOfIncomingCall(new IncomingCallEventArguments(senderPort.PhoneNumber), senderPort,
                    receiverPort);
            }
            else
            {
                OnNotifyPortOfFailure(new FailureEventArguments(e.ReceiverPhoneNumber), senderPort);
            }
        }

        public void AnswerCall(object sender, AnsweredCallEventArgs e)
        {
            var receiverPort = (Port) sender;
            var senderPort = CallsWaitingToBeAnswered.FirstOrDefault(x => x.Value == receiverPort).Key;

            CallsWaitingToBeAnswered.Remove(senderPort);

            CallsInProgress.Add(new Call(senderPort.PhoneNumber, receiverPort.PhoneNumber)
                {CallStartTime = e.CallStartTime});
        }

        public void RejectCall(object sender, RejectedCallEventArgs e)
        {
            var portRejectedCall = (Port) sender;

            Port portWhichNeedToSendNotification;

            if (CallsWaitingToBeAnswered.ContainsKey(portRejectedCall))
            {
                portWhichNeedToSendNotification = CallsWaitingToBeAnswered[portRejectedCall];

                CallsWaitingToBeAnswered.Remove(portRejectedCall);
            }
            else
            {
                portWhichNeedToSendNotification =
                    CallsWaitingToBeAnswered.FirstOrDefault(x => x.Value == portRejectedCall).Key;

                CallsWaitingToBeAnswered.Remove(portWhichNeedToSendNotification);
            }

            OnNotifyPortAboutRejectionOfCall(e, portWhichNeedToSendNotification);
        }

        protected virtual void OnNotifyPortOfIncomingCall(IncomingCallEventArguments e, Port senderPort, Port receiverPort)
        {
            try
            {
                (NotifyPortOfIncomingCall?.GetInvocationList().First(x => x.Target == receiverPort) as
                    EventHandler<IncomingCallEventArguments>)?.Invoke(this, e);
            }
            catch (Exception)
            {
                OnNotifyPortOfFailure(new FailureEventArguments(receiverPort.PhoneNumber), senderPort);
            }
        }

        protected virtual void OnNotifyPortOfFailure(FailureEventArguments e, Port port)
        {
            (NotifyPortOfFailure?.GetInvocationList().First(x => x.Target == port) as
                EventHandler<FailureEventArguments>)?.Invoke(this, e);
        }

        protected virtual void OnNotifyPortAboutRejectionOfCall(RejectedCallEventArgs e, Port port)
        {
            (NotifyPortOfRejectionOfCall?.GetInvocationList().First(x => x.Target == port) as
                EventHandler<RejectedCallEventArgs>)?.Invoke(this, e);
        }
    }
}