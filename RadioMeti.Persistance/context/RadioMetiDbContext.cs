using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RadioMeti.Domain.Entities.Log;
using RadioMeti.Domain.Entities.Music;
using RadioMeti.Domain.Entities.Prodcast;
using RadioMeti.Domain.Entities.Prodcasts;
using RadioMeti.Domain.Entities.Video;

namespace RadioMeti.Persistance.context
{
    public class RadioMetiDbContext : IdentityDbContext
    {
        public RadioMetiDbContext(DbContextOptions<RadioMetiDbContext> options) : base(options)
        {
        }
        #region log
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        #endregion
        #region Music
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<ArtistAlbum> ArtistAlbums { get; set; }
        public DbSet<ArtistMusic> ArtistMusics { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<PlayList> PlayLists { get; set; }
        public DbSet<PlayListCategory> PlayListCategories { get; set; }
        public DbSet<PlaylistMusic> PlaylistMusics { get; set; }
        public DbSet<PlayListSelectedCategory> PlayListSelectedCategories { get; set; }
        #endregion
        #region Video
        public DbSet<Video> Videos { get; set; }
        public DbSet<ArtistVideo> ArtistVideos { get; set; }
        #endregion
        #region Prodcast
        public DbSet<Dj> Djs { get; set; }
        public DbSet<Prodcast> Prodcasts { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Artist>().HasQueryFilter(p => !p.IsRemoved);

            base.OnModelCreating(builder);
        }
    }
}
