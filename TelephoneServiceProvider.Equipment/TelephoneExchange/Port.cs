namespace TelephoneServiceProvider.Equipment.TelephoneExchange
{
    public class Port
    {
        public string PhoneNumber { get; private set; }

        private BaseStation BaseStation { get; set; }

        public Port(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public void Call(string number)
        {

        }
    }
}