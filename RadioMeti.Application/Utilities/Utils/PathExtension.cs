using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.Utilities.Utils
{
   public static  class PathExtension
    {

        #region artist image
        public static string ArtistImageOriginPath = "/Content/Artist/Image/origin/";
        public static string ArtistImageOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Artist/Image/origin/");

        public static string ArtistImageThumbPath = "/Content/Artist/Image/Thumb/";
        public static string ArtistImageThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Artist/Image/Thumb/");
        #endregion

        #region artist avatar
        public static string ArtistAvatarOriginPath = "/Content/Artist/Avatar/origin/";
        public static string ArtistAvatarOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Artist/Avatar/origin/");

        public static string ArtistAvatarThumbPath = "/Content/Artist/Avatar/Thumb/";
        public static string ArtistAvatarThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Artist/Avatar/Thumb/");
        #endregion

        #region Cover Single Tracks
        public static string CoverSingleTrackOriginPath = "/Content/Cover/Music/Single/origin/";
        public static string CoverSingleTrackOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Music/Single/origin/");

        public static string CoverSingleTrackThumbPath = "/Content/Cover/Music/Single/Thumb/";
        public static string CoverSingleTrackThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Cover/Music/Single/Thumb/");

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
        #region SingleTrackAudio
        public static string AudioAlbumMusicOriginPath = "/Content/Audio/Music/AlbumMusic/origin/";
        public static string AudioAlbumMusicOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Audio/Music/AlbumMusic/origin/");


        #endregion
        
    }
}
