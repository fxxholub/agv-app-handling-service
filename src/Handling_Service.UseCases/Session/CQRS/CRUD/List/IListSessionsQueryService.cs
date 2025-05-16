
namespace Handling_Service.UseCases.Session.CQRS.CRUD.List;


public interface IListSessionsQueryService
{
  Task<IEnumerable<Handling_Service.Core.Session.SessionAggregate.Session>> ListAsync();
}
