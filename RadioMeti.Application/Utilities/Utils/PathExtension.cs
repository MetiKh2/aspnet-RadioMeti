using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.Utilities.Utils
{
    public static class PathExtension
    {

        #region artist image
        public static string ArtistImageOriginPath = "/Content/Artist/Image/origin/";
        public static string ArtistImageOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Artist/Image/origin/");

        public static string ArtistImageThumbPath = "/Content/Artist/Image/Thumb/";
        public static string ArtistImageThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Artist/Image/Thumb/");
        #endregion
        #region event cover
        public static string EventCoverOriginPath = "/Content/Event/Cover/origin/";
        public static string EventCoverOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Event/Cover/origin/");

        public static string EventCoverThumbPath = "/Content/Event/Cover/Thumb/";
        public static string EventCoverThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Event/Cover/Thumb/");
        #endregion

        #region artist avatar
        public static string ArtistAvatarOriginPath = "/Content/Artist/Avatar/origin/";
        public static string ArtistAvatarOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Artist/Avatar/origin/");

        public static string ArtistAvatarThumbPath = "/Content/Artist/Avatar/Thumb/";
        public static string ArtistAvatarThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Artist/Avatar/Thumb/");
        #endregion
        #region dj image
        public static string DjImageOriginPath = "/Content/Dj/Image/origin/";
        public static string DjImageOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Dj/Image/origin/");

        public static string DjImageThumbPath = "/Content/Dj/Image/Thumb/";
        public static string DjImageThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Dj/Image/Thumb/");
        #endregion

        #region dj avatar
        public static string DjAvatarOriginPath = "/Content/Dj/Avatar/origin/";
        public static string DjAvatarOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Dj/Avatar/origin/");

        public static string DjAvatarThumbPath = "/Content/Dj/Avatar/Thumb/";
        public static string DjAvatarThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Dj/Avatar/Thumb/");
        #endregion

        #region Cover Single Tracks
        public static string CoverSingleTrackOriginPath = "/Content/Cover/Music/Single/origin/";
        public static string CoverSingleTrackOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Music/Single/origin/");

        public static string CoverSingleTrackThumbPath = "/Content/Cover/Music/Single/Thumb/";
        public static string CoverSingleTrackThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Music/Single/Thumb/");

        #endregion
        #region Cover Prodcast
        public static string CoverProdcastOriginPath = "/Content/Cover/Prodcast/origin/";
        public static string CoverProdcastOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Prodcast/origin/");

        public static string CoverProdcastThumbPath = "/Content/Cover/Prodcast/Thumb/";
        public static string CoverProdcastThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Prodcast/Thumb/");

        #endregion

        #region Cover Album
        public static string CoverAlbumOriginPath = "/Content/Cover/Music/Album/origin/";
        public static string CoverAlbumOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Music/Album/origin/");

        public static string CoverAlbumThumbPath = "/Content/Cover/Music/Album/Thumb/";
        public static string CoverAlbumThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Music/Album/Thumb/");

        #endregion  
        #region Cover Album Music
        public static string CoverAlbumMusicOriginPath = "/Content/Cover/Music/AlbumMusic/origin/";
        public static string CoverAlbumMusicOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Music/AlbumMusic/origin/");

        public static string CoverAlbumMusicThumbPath = "/Content/Cover/Music/AlbumMusic/Thumb/";
        public static string CoverAlbumMusicThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Music/AlbumMusic/Thumb/");

        #endregion
        #region SingleTrackAudio
        public static string AudioSingleTrackOriginPath = "/Content/Audio/Music/Single/origin/";
        public static string AudioSingleTrackOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Audio/Music/Single/origin/");


        #endregion
        #region Album music Audio
        public static string AudioAlbumMusicOriginPath = "/Content/Audio/Music/AlbumMusic/origin/";
        public static string AudioAlbumMusicOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Audio/Music/AlbumMusic/origin/");


        #endregion
        #region Prodcast Audio
        public static string AudioProdcastOriginPath = "/Content/Audio/Prodcast/origin/";
        public static string AudioProdcastOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Audio/Prodcast/origin/");


        #endregion
        #region Video
        public static string VideoOriginPath = "/Content/Video/origin/";
        public static string VideoOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Video/origin/");


        #endregion
        #region Cover playlist category
        public static string CoverPlaylistCategoryOriginPath = "/Content/Cover/Playlist/Category/origin/";
        public static string CoverPlaylistCategoryOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Playlist/Category/origin/");

        public static string CoverPlaylistCategoryThumbPath = "/Content/Cover/Playlist/Category/Thumb/";
        public static string CoverPlaylistCategoryThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Playlist/Category/Thumb/");

        #endregion
        #region Cover video
        public static string CoverVideoOriginPath = "/Content/Cover/Video/origin/";
        public static string CoverVideoOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Video/origin/");

        public static string CoverVideoThumbPath = "/Content/Cover/Video/Thumb/";
        public static string CoverVideoThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Video/Thumb/");

        #endregion
        #region Cover playlist 
        public static string CoverPlaylistOriginPath = "/Content/Cover/Playlist/origin/";
        public static string CoverPlaylistOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Playlist/origin/");

        public static string CoverPlaylistThumbPath = "/Content/Cover/Playlist/Thumb/";
        public static string CoverPlaylistThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Playlist/Thumb/");

        #endregion
    }
}
