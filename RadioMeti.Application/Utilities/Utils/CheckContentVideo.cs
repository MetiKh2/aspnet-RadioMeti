using Microsoft.AspNetCore.Http;

namespace RadioMeti.Application.Utilities.Utils
{
    public static class CheckContentVideo
    {
        public static bool IsVideo(this IFormFile postedFile)
        {
            //-------------------------------------------
            //  Check the video mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "video/3gpp"
                      && postedFile.ContentType.ToLower() != "video/m4v"
                      && postedFile.ContentType.ToLower() != "video/mp4"
                      && postedFile.ContentType.ToLower() != "video/mpeg"
                      && postedFile.ContentType.ToLower() != "video/ogg"
                      && postedFile.ContentType.ToLower() != "video/webm"
                      && postedFile.ContentType.ToLower() != "video/quicktime"
                      && postedFile.ContentType.ToLower() != "video/x-ms-wmv"
                        )
            {
                return false;
            }

            //-------------------------------------------
            //  Check the video extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".mp4"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".mov"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".avi"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".flv"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".mkv"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".avchd"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".webm"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".mpeg-4"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".wmv")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            if (!postedFile.OpenReadStream().CanRead)
            {
                return false;
            }
            return true;
        }
    }
}
