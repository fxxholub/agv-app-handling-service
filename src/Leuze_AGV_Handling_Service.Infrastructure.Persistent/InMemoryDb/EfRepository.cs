using Ardalis.SharedKernel;
using Ardalis.Specification.EntityFrameworkCore;
using Leuze_AGV_Handling_Service.Infrastructure.Persistent.InMemoryDb;

namespace Leuze_AGV_Handling_Service.Infrastructure.Persistent.InMemoryDb;

// inherit from Ardalis.Specification type
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
  public EfRepository(AppDbContext dbContext) : base(dbContext)
  {
  }
}
