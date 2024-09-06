
namespace Leuze_AGV_Handling_Service.UseCases.Session.List;


public interface IListSessionsQueryService
{
  Task<IEnumerable<SessionDTO>> ListAsync();
}
