using Microsoft.EntityFrameworkCore;

using MVMedia.Api.Models;

namespace MVMedia.Api.Context;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> opt) : base(opt){}

    public DbSet<Client> Clients { get; set; }
    public DbSet<Media> Medias { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<MediaFile> MediaFiles { get; set; }
    public DbSet<MediaList> MediaLists { get; set; }
    public DbSet<Company> Companies { get; set; }

}
