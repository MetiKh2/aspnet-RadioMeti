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

    }
}
