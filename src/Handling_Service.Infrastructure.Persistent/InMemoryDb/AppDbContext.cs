using System.Reflection;
using Ardalis.SharedKernel;
using Handling_Service.Core.Session.SessionAggregate;
using Microsoft.EntityFrameworkCore;

namespace Handling_Service.Infrastructure.Persistent.InMemoryDb;

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

        // Session has Actions relationship
        modelBuilder.Entity<Session>()
            .Navigation(s => s.Actions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        // Session has Processes relationship
        modelBuilder.Entity<Session>()
            .Navigation(s => s.Processes)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Processes has Commands relationship
        modelBuilder.Entity<Process>()
            .HasMany(p => p.Commands)  // Process has many Commands
            .WithOne()  // Command has one Process (implicitly)
            .HasForeignKey(c => c.ProcessId);  // Use ProcessId as the foreign key in BashCommand
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