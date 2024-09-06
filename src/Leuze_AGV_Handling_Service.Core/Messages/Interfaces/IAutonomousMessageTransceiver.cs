using Ardalis.Result;

namespace Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;

public interface IAutonomousMessageTransceiver
{
    public Task ReceiveMap(string message);
    
    public Task<Result> SendJoy(string message);
}