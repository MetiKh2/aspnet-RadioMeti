using RadioMeti.Application.DTOs.Admin.Artists;
using RadioMeti.Application.DTOs.Admin.Artists.Create;
using RadioMeti.Application.DTOs.Admin.Artists.Delete;
using RadioMeti.Application.DTOs.Admin.Artists.Edit;
using RadioMeti.Domain.Entities.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.Interfaces
{
    public interface IArtistService
    {
        Task<CreateArtistResult> CreateArtist(CreateArtistDto create);
        Task<EditArtistResult> EditArtist(EditArtistDto edit);
        Task<DeleteArtistResult> DeleteArtist(long id);
        Task<FilterArtistsDto> FilterArtists(FilterArtistsDto filter);
        Task<Artist> GetArtistBy(long id);
        Task<List<Artist>> GetArtists();
    }
}
