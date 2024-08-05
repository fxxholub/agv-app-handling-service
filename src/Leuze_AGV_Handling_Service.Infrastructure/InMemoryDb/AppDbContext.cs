using System.Reflection;
using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.SessionAggregate;
using Microsoft.EntityFrameworkCore;

namespace Leuze_AGV_Handling_Service.Infrastructure.InMemoryDb;

public class AppDbContext : DbContext
{
    private readonly IDomainEventDispatcher? _dispatcher;

    public AppDbContext(DbContextOptions<AppDbContext> options,
        IDomainEventDispatcher? dispatcher)
        : base(options)
    {
        _dispatcher = dispatcher;
    }

    // add entities here
    public DbSet<Session> Sessions => Set<Session>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Session>()
            .Navigation(s => s.Processes)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // var navigation = modelBuilder.Entity<Session>()
        //     .Metadata.FindNavigation(nameof(Session.Processes));
        // navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // ignore events if no dispatcher provided
        if (_dispatcher == null) return result;

        // dispatch events only if save was successful
        var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

        return result;
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }
}