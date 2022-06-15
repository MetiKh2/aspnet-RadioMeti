using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RadioMeti.Domain.Entities.Event;
using RadioMeti.Domain.Entities.Log;
using RadioMeti.Domain.Entities.Music;
using RadioMeti.Domain.Entities.Prodcast;
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
        public DbSet<UserMusicLike> UserMusicLikes{ get; set; }
        #endregion
        #region Video
        public DbSet<Video> Videos { get; set; }
        public DbSet<ArtistVideo> ArtistVideos { get; set; }
        public DbSet<UserVideoLike> UserVideoLikes{ get; set; }
        #endregion
        #region Prodcast
        public DbSet<Dj> Djs { get; set; }
        public DbSet<Prodcast> Prodcasts { get; set; }
        public DbSet<UserProdcastLike> UserProdcastLikes{ get; set; }
        #endregion
        #region Event
        public DbSet<Event> Events{ get; set; }
        public DbSet<ArtistEvent> ArtistEvents{ get; set; }

        #endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Artist>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<Music>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<ArtistMusic>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<Album>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<ArtistAlbum>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<PlayList>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<PlayListCategory>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<PlaylistMusic>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<PlayListSelectedCategory>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<Video>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<ArtistVideo>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<Dj>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<Prodcast>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<Event>().HasQueryFilter(p => !p.IsRemoved);
            builder.Entity<ArtistEvent>().HasQueryFilter(p => !p.IsRemoved);

            base.OnModelCreating(builder);
        }
    }
}
