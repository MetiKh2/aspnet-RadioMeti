 
namespace RadioMeti.Application.DTOs.Video
{
    public class ShowVideoPageDto
    {
        public Domain.Entities.Video.Video Video { get; set; }
        public List<Domain.Entities.Video.Video> RelatedVideos { get; set; }
    }
}
