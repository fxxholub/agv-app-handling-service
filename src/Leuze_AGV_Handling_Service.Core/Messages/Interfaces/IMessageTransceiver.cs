namespace Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;

public interface IMessageTransceiver
{
    public Task EnableTransmission();

    public Task DisableTransmission();
}