namespace TelephoneServiceProvider.Core.Contracts.Clients
{
    public interface IClientEquipment
    {
        ITerminal Terminal { get; }

        IPort Port { get; }
    }
}