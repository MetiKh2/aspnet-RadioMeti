

using RadioMeti.Application.DTOs.Admin.Event;
using RadioMeti.Application.DTOs.Admin.Event.Create;
using RadioMeti.Application.DTOs.Admin.Event.Delete;
using RadioMeti.Application.DTOs.Admin.Event.Edit;
using RadioMeti.Domain.Entities.Event;

namespace RadioMeti.Application.Interfaces
{
    public interface IEventService
    {
        Task<FilterEventDto> FilterEvent(FilterEventDto filter);
        Task<Tuple<CreateEventResult,long>> CreateEvent(CreateEventDto create);
        Task<EditEventResult> EditEvent(EditEventDto edit);
        Task<Event> GetEventBy(long id);
        Task<List<long>> GetArtistsEvent(long eventId);
        Task CreateEventArtists(long eventId, List<long> selectedArtists);
        Task DeleteEventArtists(long eventId);
        Task<DeleteEventResult> DeleteEvent(long id);
    }
}
