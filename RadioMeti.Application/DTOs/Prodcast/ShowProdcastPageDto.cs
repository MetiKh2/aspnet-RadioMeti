 

namespace RadioMeti.Application.DTOs.Prodcast
{
    public class ShowProdcastPageDto
    {
        public Domain.Entities.Prodcast.Prodcast Prodcast { get; set; }
        public List<Domain.Entities.Prodcast.Prodcast> RelatedProdcasts{ get; set; }
    }
}
