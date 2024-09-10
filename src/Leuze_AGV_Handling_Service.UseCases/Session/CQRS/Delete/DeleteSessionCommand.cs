using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.Delete;

public record DeleteSessionCommand(int SessionId) : ICommand<Result>;
