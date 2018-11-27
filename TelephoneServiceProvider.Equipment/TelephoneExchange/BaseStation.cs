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
        public event EventHandler<IncomingCallEventArgs> NotifyPortOfIncomingCall;

        public event EventHandler<RejectedCallEventArgs> NotifyPortOfRejectionOfCall;

        public event EventHandler<FailureEventArgs> NotifyPortOfFailure;

        public IList<Port> Ports { get; }

        public IDictionary<Port, Port> CallsWaitingToBeAnswered { get; private set; }

        public IList<Call> CallsInProgress { get; private set; }

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

                OnNotifyPortOfIncomingCall(new IncomingCallEventArgs(senderPort.PhoneNumber), senderPort,
                    receiverPort);
            }
            else
            {
                OnNotifyPortOfFailure(new FailureEventArgs(e.ReceiverPhoneNumber), senderPort);
            }
        }

        public void AnswerCall(object sender, AnsweredCallEventArgs e)
        {
            var receiverPort = (Port) sender;
            var senderPort = CallsWaitingToBeAnswered.FirstOrDefault(x => x.Value == receiverPort).Key;

            if(senderPort == null) return;

            CallsWaitingToBeAnswered.Remove(senderPort);

            CallsInProgress.Add(new Call(senderPort.PhoneNumber, receiverPort.PhoneNumber)
                {CallStartTime = e.CallStartTime});
        }

        public void RejectCall(object sender, RejectedCallEventArgs e)
        {
            var portRejectedCall = (Port) sender;

            Port portWhichNeedToSendNotification;

            var canceledCall =
                CallsInProgress.FirstOrDefault(x =>
                    x.ReceiverPhoneNumber == portRejectedCall.PhoneNumber ||
                    x.SenderPhoneNumber == portRejectedCall.PhoneNumber);

            if (canceledCall != null)
            {
                portWhichNeedToSendNotification = canceledCall.SenderPhoneNumber == portRejectedCall.PhoneNumber
                    ? Ports.FirstOrDefault(x => x.PhoneNumber == canceledCall.ReceiverPhoneNumber)
                    : Ports.FirstOrDefault(x => x.PhoneNumber == canceledCall.SenderPhoneNumber);

                CallsInProgress.Remove(canceledCall);

                canceledCall.CallEndTime = e.CallRejectionTime;
            }
            else
            {
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
            }

            OnNotifyPortAboutRejectionOfCall(e, portWhichNeedToSendNotification);
        }

        protected virtual void OnNotifyPortOfIncomingCall(IncomingCallEventArgs e, Port senderPort, Port receiverPort)
        {
            try
            {
                (NotifyPortOfIncomingCall?.GetInvocationList().First(x => x.Target == receiverPort) as
                    EventHandler<IncomingCallEventArgs>)?.Invoke(this, e);
            }
            catch (Exception)
            {
                OnNotifyPortOfFailure(new FailureEventArgs(receiverPort.PhoneNumber), senderPort);
            }
        }

        protected virtual void OnNotifyPortOfFailure(FailureEventArgs e, Port port)
        {
            (NotifyPortOfFailure?.GetInvocationList().First(x => x.Target == port) as
                EventHandler<FailureEventArgs>)?.Invoke(this, e);
        }

        protected virtual void OnNotifyPortAboutRejectionOfCall(RejectedCallEventArgs e, Port port)
        {
            (NotifyPortOfRejectionOfCall?.GetInvocationList().First(x => x.Target == port) as
                EventHandler<RejectedCallEventArgs>)?.Invoke(this, e);
        }
    }
}