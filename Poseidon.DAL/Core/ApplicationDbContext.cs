namespace Poseidon.DAL.Core;

public class ApplicationDbContext : IdentityDbContext<ApplicationUserEntity>
{
    public DbSet<VesselEntity> Vessels { get; set; }

    public DbSet<EventEntity> Events { get; set; }

    public DbSet<LatestEventEntity> LatestEvents { get; set; }
    
    public ApplicationDbContext(DbContextOptions contextOptions) : base(contextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<VesselEntity>().Property(vessel => vessel.Id).ValueGeneratedOnAdd();
        builder.Entity<EventEntity>().Property(@event => @event.Id).ValueGeneratedOnAdd();
        builder.Entity<LatestEventEntity>().Property(latestEvent => latestEvent.Id).ValueGeneratedOnAdd();
    }
}
