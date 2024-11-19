
namespace Leuze_AGV_Handling_Service.UseCases.Session.CQRS.CRUD.List;


public interface IListSessionsQueryService
{
  Task<IEnumerable<Core.Session.SessionAggregate.Session>> ListAsync();
}
