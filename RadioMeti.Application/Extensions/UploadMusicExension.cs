using Microsoft.AspNetCore.Http;
using RadioMeti.Application.Utilities.Utils;
namespace RadioMeti.Application.Extensions
{
    public static class UploadMusicExension
    {
        public static bool AddAudioToServer(this IFormFile audio, string fileName, string orginalPath,string deletefileName = null)
        {
            if (audio != null && audio.IsAudio())
            {
                if (!Directory.Exists(orginalPath))
                    Directory.CreateDirectory(orginalPath);

                if (!string.IsNullOrEmpty(deletefileName))
                {
                    if (File.Exists(orginalPath + deletefileName))
                        File.Delete(orginalPath + deletefileName);

                }
                string OriginPath = orginalPath + fileName;
                using (var stream = new FileStream(OriginPath, FileMode.Create))
                {
                    if (!Directory.Exists(OriginPath)) audio.CopyTo(stream);
                }
                return true;
            }
            else return false;
        }

    }
}
