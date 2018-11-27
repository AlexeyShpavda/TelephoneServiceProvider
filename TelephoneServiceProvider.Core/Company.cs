namespace TelephoneServiceProvider.Core
{
    public class Company
    {
        public string Name { get; private set; }

        public Company(string name)
        {
            Name = name;
        }
    }
}