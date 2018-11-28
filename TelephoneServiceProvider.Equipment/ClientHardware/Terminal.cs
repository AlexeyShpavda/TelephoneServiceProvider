﻿using System;
using TelephoneServiceProvider.Equipment.Contracts.ClientHardware;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange;
using TelephoneServiceProvider.Equipment.Contracts.TelephoneExchange.EventsArgs;
using TelephoneServiceProvider.Equipment.TelephoneExchange.EventsArgs;

namespace TelephoneServiceProvider.Equipment.ClientHardware
{
    public class Terminal : ITerminal
    {
        public Action<string> DisplayMethod { get; private set; }

        public event EventHandler ConnectedToPort;

        public event EventHandler DisconnectedFromPort;

        public event EventHandler<IRejectedCallEventArgs> NotifyPortAboutRejectionOfCall;

        public event EventHandler<IAnsweredCallEventArgs> NotifyPortAboutAnsweredCall;

        public Guid SerialNumber { get; }

        public bool IsConnectedWithPort { get; private set; }

        public IPort Port { get; set; }

        public Terminal()
        {
            DisplayMethod = null;
            SerialNumber = Guid.NewGuid();
            IsConnectedWithPort = false;
            Port = null;

            //Mapping.SubscribeToSyncWithTerminal(this, Port);
        }

        public void SetDisplayMethod(Action<string> action)
        {
            DisplayMethod = action;
        }

        public void ConnectToPort(IPort port)
        {
            if (port == null) return;

            Port = port;
            IsConnectedWithPort = true;
            OnConnectedToPort();
        }

        public void DisconnectFromPort()
        {
            Port = null;
            IsConnectedWithPort = false;
            OnDisconnectedFromPort();
        }

        public void Call(string receiverPhoneNumber)
        {
            if (IsConnectedWithPort)
            {
                Port.OutgoingCall(receiverPhoneNumber);
            }
        }

        public void Answer()
        {
            DisplayMethod?.Invoke("You Answered Call");

            OnNotifyPortAboutAnsweredCall(new AnsweredCallEventArgs("") { CallStartTime = DateTime.Now });
        }

        public void Reject()
        {
            DisplayMethod?.Invoke("You Rejected Call");

            OnNotifyPortAboutRejectionOfCall(new RejectedCallEventArgs("") { CallRejectionTime = DateTime.Now });
        }

        public void NotifyUserAboutError(object sender, IFailureEventArgs e)
        {
            DisplayMethod?.Invoke($"{e.ReceiverPhoneNumber} - Subscriber Doesn't Exist or He is Busy");
        }

        public void NotifyUserAboutIncomingCall(object sender, IIncomingCallEventArgs e)
        {
            DisplayMethod?.Invoke($"{e.SenderPhoneNumber} - is calling you");
        }

        public void NotifyUserAboutRejectedCall(object sender, IRejectedCallEventArgs e)
        {
            DisplayMethod?.Invoke($"{e.PhoneNumberOfPersonRejectedCall} - canceled the call");
        }

        protected virtual void OnConnectedToPort()
        {
            ConnectedToPort?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDisconnectedFromPort()
        {
            DisconnectedFromPort?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnNotifyPortAboutRejectionOfCall(IRejectedCallEventArgs e)
        {
            NotifyPortAboutRejectionOfCall?.Invoke(this, e);
        }

        protected virtual void OnNotifyPortAboutAnsweredCall(IAnsweredCallEventArgs e)
        {
            NotifyPortAboutAnsweredCall?.Invoke(this, e);
        }
    }
}