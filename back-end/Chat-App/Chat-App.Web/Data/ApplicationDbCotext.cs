namespace Chat_App.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<Message> Messages { get; set; }
	public DbSet<Reaction> Reactions { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Configuring one-to-many relationship between Message and Reactions
		modelBuilder.Entity<Reaction>()
			.HasOne(r => r.Message)
			.WithMany(m => m.Reactions)
			.HasForeignKey(r => r.MessageId);
	}
}
