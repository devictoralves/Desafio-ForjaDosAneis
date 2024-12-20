using ForjaDosAneis.Api.Models;
using Microsoft.EntityFrameworkCore;

public class AnelDbContext : DbContext
{
    public DbSet<Anel> Aneis { get; set; }

    public AnelDbContext(DbContextOptions<AnelDbContext> options) : base(options) { }
}