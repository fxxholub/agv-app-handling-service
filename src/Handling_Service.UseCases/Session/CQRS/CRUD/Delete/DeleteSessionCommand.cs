using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Handling_Service.UseCases.Session.CQRS.CRUD.Delete;

/// <summary>
/// Deletes Session. Entity repository deletion operation.
/// </summary>
/// <param name="SessionId"></param>
public record DeleteSessionCommand(int SessionId) : ICommand<Result>;
