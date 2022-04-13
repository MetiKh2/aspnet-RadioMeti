using Microsoft.AspNetCore.Http;
using RadioMeti.Application.Utilities.Utils;

namespace RadioMeti.Application.Extensions
{
    public static class UploadVideoExtension
    {
        public static bool AddVideoToServer(this IFormFile video, string fileName, string orginalPath, string deletefileName = null)
        {
            if (video != null && video.IsVideo())
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
                    if (!Directory.Exists(OriginPath)) video.CopyTo(stream);
                }
                return true;
            }
            else return false;
        }
    }
}
