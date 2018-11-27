using System.Collections.Generic;

namespace TelephoneServiceProvider.Equipment.TelephoneExchange
{
    public class BaseStation
    {
        public IEnumerable<Port> Ports { get; }

        public BaseStation(IEnumerable<Port> ports)
        {
            Ports = ports;
        }
    }
}