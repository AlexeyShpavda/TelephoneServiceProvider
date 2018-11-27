using System;
using System.Collections.Generic;
using System.Linq;
using TelephoneServiceProvider.Equipment.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange
{
    public class BaseStation
    {
        public event EventHandler<IncomingCallEventArguments> NotifyPortOfIncomingCall;

        public event EventHandler<FailureEventArguments> NotifyPortOfFailure;

        public IList<Port> Ports { get; }

        public readonly IDictionary<Port, Port> CallsWaitingToBeAnswered;

        public BaseStation(IList<Port> ports)
        {
            CallsWaitingToBeAnswered = new Dictionary<Port, Port>();
            Ports = ports;
        }

        public void NotifyIncomingCallPort(object sender, OutgoingCallEventArgs e)
        {
            var senderPort = (Port) sender;
            var receiverPort = Ports.FirstOrDefault(x => x.PhoneNumber == e.ReceiverPhoneNumber);

            if (receiverPort != null && receiverPort.PortStatus == PortStatus.Free)
            {
                CallsWaitingToBeAnswered.Add(senderPort, receiverPort);

                OnNotifyPortOfIncomingCall(new IncomingCallEventArguments(senderPort.PhoneNumber), receiverPort);
            }
            else
            {
                OnNotifyPortOfFailure(new FailureEventArguments(e.ReceiverPhoneNumber));
            }
        }

        protected virtual void OnNotifyPortOfIncomingCall(IncomingCallEventArguments e, Port receiverPort)
        {
            try
            {
                (NotifyPortOfIncomingCall?.GetInvocationList().First(x => x.Target == receiverPort) as
                    EventHandler<IncomingCallEventArguments>)?.Invoke(this, e);
            }
            catch (Exception)
            {
                OnNotifyPortOfFailure(new FailureEventArguments(receiverPort.PhoneNumber));
            }
        }

        protected virtual void OnNotifyPortOfFailure(FailureEventArguments e)
        {
            NotifyPortOfFailure?.Invoke(this, e);
        }
    }
}