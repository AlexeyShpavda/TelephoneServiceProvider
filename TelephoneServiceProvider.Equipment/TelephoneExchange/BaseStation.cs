using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using TelephoneServiceProvider.BillingSystem.Contracts.EventArgs;
using TelephoneServiceProvider.BillingSystem.Contracts.Repositories.Entities;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.BaseStation;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Enums;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.Port;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange
{
    public class BaseStation : IBaseStationCore, IBaseStationEvents
    {
        public int CancellationTime { get; }

        public event EventHandler<IncomingCallEventArgs> NotifyPortOfIncomingCall;

        public event EventHandler<RejectedCallEventArgs> NotifyPortOfRejectionOfCall;

        public event EventHandler<FailureEventArgs> NotifyPortOfFailure;

        public event EventHandler<CallEventArgs> NotifyBillingSystemAboutCallEnd;

        public event EventHandler<CheckBalanceEventArgs> CheckBalanceInBillingSystem;

        public IList<IPortCore> Ports { get; }

        public IDictionary<IPortCore, IPortCore> CallsWaitingToBeAnswered { get; }

        public IDictionary<IPortCore, Timer> PortTimeout { get; }

        public IList<ICall> CallsInProgress { get; }

        public BaseStation(int cancellationTime = 4000)
        {
            CallsWaitingToBeAnswered = new Dictionary<IPortCore, IPortCore>();
            PortTimeout = new Dictionary<IPortCore, Timer>();
            CallsInProgress = new List<ICall>();
            Ports = new List<IPortCore>();
            CancellationTime = cancellationTime;
        }

        public void AddPorts(IEnumerable<IPortCore> ports)
        {
            foreach (var port in ports)
            {
                AddPort(port);
            }
        }

        public void RemovePorts(IEnumerable<IPortCore> ports)
        {
            foreach (var port in ports)
            {
                RemovePort(port);
            }
        }

        public void AddPort(IPortCore port)
        {
            Mapping.ConnectPortToStation(port as IPortEvents, this);

            Ports.Add(port);

            Logger.WriteLine($"{port.PhoneNumber} was Attached to Station");
        }

        public void RemovePort(IPortCore port)
        {
            Mapping.DisconnectPortFromStation(port as IPortEvents, this);

            Ports.Remove(port);


            Logger.WriteLine($"{port.PhoneNumber} was Disconnected from Station");
        }

        public void NotifyIncomingCallPort(object sender, OutgoingCallEventArgs e)
        {
            var senderPort = sender as IPortCore;

            Logger.WriteLine($"{e.SenderPhoneNumber} is Calling {e.ReceiverPhoneNumber}");

            var checkBalanceEventArgs = new CheckBalanceEventArgs(e.SenderPhoneNumber);
            OnCheckBalanceInBillingSystem(checkBalanceEventArgs);

            Logger.WriteLine($"Billing System Checks {e.SenderPhoneNumber} Balance");

            if (checkBalanceEventArgs.IsAllowedCall)
            {
                Logger.WriteLine($"{e.SenderPhoneNumber} has Enough Money to Make Call");

                ConnectPorts(senderPort, e);
            }
            else
            {
                Logger.WriteLine($"{e.SenderPhoneNumber} has not Enough Money to Make Call");

                OnNotifyPortOfFailure(new FailureEventArgs(e.ReceiverPhoneNumber, FailureType.InsufficientFunds),
                    senderPort);
            }
        }

        private void ConnectPorts(IPortCore senderPort, OutgoingCallEventArgs e)
        {
            var receiverPort = Ports.FirstOrDefault(x => x.PhoneNumber == e.ReceiverPhoneNumber);

            if (receiverPort == null || senderPort == null)
            {
                OnNotifyPortOfFailure(new FailureEventArgs(e.ReceiverPhoneNumber, FailureType.SubscriberDoesNotExist),
                    senderPort);

                Logger.WriteLine($"{e.ReceiverPhoneNumber} Does not Exist");
            }
            else if (receiverPort.PortStatus != PortStatus.Free)
            {
                OnNotifyPortOfFailure(new FailureEventArgs(e.ReceiverPhoneNumber, FailureType.SubscriberIsBusy),
                    senderPort);

                Logger.WriteLine($"{e.ReceiverPhoneNumber} is Busy");
            }
            else
            {
                CallsWaitingToBeAnswered.Add(senderPort, receiverPort);

                PortTimeout.Add(senderPort, SetTimer(senderPort, receiverPort));

                OnNotifyPortOfIncomingCall(new IncomingCallEventArgs(senderPort.PhoneNumber), receiverPort);
            }
        }

        private Timer SetTimer(IPortCore senderPort, IPortCore receiverPort)
        {
            var timer = new Timer(CancellationTime);

            timer.Elapsed += (sender, eventArgs) =>
            {
                Logger.WriteLine($"{receiverPort.PhoneNumber} Did not Answer Call from {senderPort.PhoneNumber}");

                OnNotifyPortOfFailure(
                    new FailureEventArgs(receiverPort.PhoneNumber, FailureType.SubscriberIsNotResponding),
                    senderPort);

                OnNotifyPortOfFailure(
                    new FailureEventArgs(receiverPort.PhoneNumber, FailureType.SubscriberIsNotResponding),
                    receiverPort);

                CallsWaitingToBeAnswered.Remove(senderPort);
                DisposeTimer(senderPort);

                Logger.WriteLine("Base Station Notifies Billing System of Failed Call " +
                                 $"from {senderPort.PhoneNumber} to {receiverPort.PhoneNumber}");

                OnNotifyBillingSystemAboutCallEnd(new UnansweredCallEventArgs(senderPort.PhoneNumber,
                    receiverPort.PhoneNumber, DateTime.Now));
            };

            timer.AutoReset = false;
            timer.Enabled = true;

            return timer;
        }

        private void DisposeTimer(IPortCore port)
        {
            PortTimeout[port].Dispose();
            PortTimeout.Remove(port);
        }

        public void AnswerCall(object sender, AnsweredCallEventArgs e)
        {
            if (!(sender is IPortCore receiverPort)) return;

            var senderPort = CallsWaitingToBeAnswered.FirstOrDefault(x => x.Value == receiverPort).Key;

            if (senderPort == null) return;

            DisposeTimer(senderPort);
            CallsWaitingToBeAnswered.Remove(senderPort);

            CallsInProgress.Add(new HeldCallEventArgs(senderPort.PhoneNumber, receiverPort.PhoneNumber)
            { CallStartTime = e.CallStartTime });

            Logger.WriteLine($"{receiverPort.PhoneNumber} Answered Call from {senderPort.PhoneNumber}");
        }

        public void RejectCall(object sender, RejectedCallEventArgs e)
        {
            if (!(sender is IPortCore portRejectedCall)) return;

            var suitableCall = CallsInProgress.FirstOrDefault(x =>
                x.ReceiverPhoneNumber == portRejectedCall.PhoneNumber ||
                x.SenderPhoneNumber == portRejectedCall.PhoneNumber);

            var portWhichNeedToSendNotification = suitableCall is IAnsweredCall answeredCall
                ? CompleteCallInProgress(portRejectedCall, answeredCall, e)
                : CancelNotStartedCall(portRejectedCall, e);

            OnNotifyPortAboutRejectionOfCall(e, portWhichNeedToSendNotification);
        }

        private IPortCore CompleteCallInProgress(IPortCore portRejectedCall, IAnsweredCall call, RejectedCallEventArgs e)
        {
            var portWhichNeedToSendNotification = call.SenderPhoneNumber == portRejectedCall.PhoneNumber
                ? Ports.FirstOrDefault(x => x.PhoneNumber == call.ReceiverPhoneNumber)
                : Ports.FirstOrDefault(x => x.PhoneNumber == call.SenderPhoneNumber);

            if (portWhichNeedToSendNotification != null) Logger.WriteLine(
                $"{portRejectedCall.PhoneNumber} Ended Call with {portWhichNeedToSendNotification.PhoneNumber}");

            CallsInProgress.Remove(call);

            Logger.WriteLine("Base Station Notifies Billing System of Completed call " +
                             $"from {call.SenderPhoneNumber} to {call.ReceiverPhoneNumber}");

            OnNotifyBillingSystemAboutCallEnd(new HeldCallEventArgs(call.SenderPhoneNumber, call.ReceiverPhoneNumber,
                call.CallStartTime, e.CallRejectionTime));

            return portWhichNeedToSendNotification;
        }

        private IPortCore CancelNotStartedCall(IPortCore portRejectedCall, RejectedCallEventArgs e)
        {
            IPortCore portWhichNeedToSendNotification;
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

            DisposeTimer(portRejectedCall);

            OnNotifyBillingSystemAboutCallEnd(new UnansweredCallEventArgs(senderPhoneNumber, receiverPhoneNumber,
                e.CallRejectionTime));

            Logger.WriteLine(
                $"{portRejectedCall.PhoneNumber} Rejected Call from {portWhichNeedToSendNotification.PhoneNumber}");

            return portWhichNeedToSendNotification;
        }

        private void OnNotifyPortOfIncomingCall(IncomingCallEventArgs e, IPortCore port)
        {
            if (NotifyPortOfIncomingCall?.GetInvocationList().FirstOrDefault(x => x.Target == port) != null)
            {
                (NotifyPortOfIncomingCall?.GetInvocationList().FirstOrDefault(x => x.Target == port) as
                    EventHandler<IncomingCallEventArgs>)?.Invoke(this, e);
            }
        }

        private void OnNotifyPortOfFailure(FailureEventArgs e, IPortCore port)
        {
            if (NotifyPortOfFailure?.GetInvocationList().FirstOrDefault(x => x.Target == port) != null)
            {
                (NotifyPortOfFailure?.GetInvocationList().First(x => x.Target == port) as
                    EventHandler<FailureEventArgs>)?.Invoke(this, e);
            }
        }

        private void OnNotifyPortAboutRejectionOfCall(RejectedCallEventArgs e, IPortCore port)
        {
            if (NotifyPortOfRejectionOfCall?.GetInvocationList().FirstOrDefault(x => x.Target == port) != null)
            {
                (NotifyPortOfRejectionOfCall?.GetInvocationList().First(x => x.Target == port) as
                    EventHandler<RejectedCallEventArgs>)?.Invoke(this, e);
            }
        }

        private void OnNotifyBillingSystemAboutCallEnd(CallEventArgs e)
        {
            NotifyBillingSystemAboutCallEnd?.Invoke(this, e);
        }

        private void OnCheckBalanceInBillingSystem(CheckBalanceEventArgs e)
        {
            CheckBalanceInBillingSystem?.Invoke(this, e);
        }
    }
}