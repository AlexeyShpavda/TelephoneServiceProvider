using System;
using System.Collections.Generic;
using TelephoneServiceProvider.Equipment.ClientHardware;
using TelephoneServiceProvider.Equipment.TelephoneExchange;

namespace TelephoneServiceProvider.PresentationLayer
{
    internal class Program
    {
        private static void Main()
        {
            var port1 = new Port("1");
            var terminal1 = new Terminal(port1);


            var port2 = new Port("2");
            var terminal2 = new Terminal(port2);


            var baseStation = new BaseStation(new List<Port> {port1, port2});

            terminal1.Call("2");

        }
    }
}
