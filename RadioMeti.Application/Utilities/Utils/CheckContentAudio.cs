using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RadioMeti.Application.Utilities.Utils
{
    public static class CheckContentAudio
    {
       

        public static bool IsAudio(this IFormFile postedFile)
        {
            //-------------------------------------------
            //  Check the audio mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "audio/mp3" &&
                        postedFile.ContentType.ToLower() != "audio/ogg" &&
                        postedFile.ContentType.ToLower() != "audio/vnd.rn-realaudio" &&
                        postedFile.ContentType.ToLower() != "audio/mpeg" &&
                        postedFile.ContentType.ToLower() != "audio/vnd.wave" &&
                        postedFile.ContentType.ToLower() != "audio/x-aiff" &&
                        postedFile.ContentType.ToLower() != "audio/basic" &&
                        postedFile.ContentType.ToLower() != "audio/midi" &&
                        postedFile.ContentType.ToLower() != "audio/x-mpegurl" &&
                        postedFile.ContentType.ToLower() != "audio/mp4a-latm" &&
                        postedFile.ContentType.ToLower() != "audio/mesh" &&
                        postedFile.ContentType.ToLower() != "audio/x-pn-realaudio" &&
                        postedFile.ContentType.ToLower() != "audio/x-wav" &&
                        postedFile.ContentType.ToLower() != "audio/vorbis")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the audio extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".mp3"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".aac"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".flac"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".alf"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".ogg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".wav")
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
