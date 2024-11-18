using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.Delete;

public record DeleteSessionCommand(int SessionId) : ICommand<Result>;
