using Ardalis.Result;

namespace Leuze_AGV_Handling_Service.UseCases.Messages.Interfaces;

public interface IAutonomousMessageNotifier
{
    Task<Result> NotifyReceiveMap(string message);
}