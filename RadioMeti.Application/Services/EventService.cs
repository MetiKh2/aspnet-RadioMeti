using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RadioMeti.Application.DTOs.Admin.Event;
using RadioMeti.Application.DTOs.Admin.Event.Create;
using RadioMeti.Application.DTOs.Admin.Event.Delete;
using RadioMeti.Application.DTOs.Admin.Event.Edit;
using RadioMeti.Application.DTOs.Paging;
using RadioMeti.Application.Interfaces;
using RadioMeti.Domain.Entities.Event;
using RadioMeti.Persistance.Repository;

namespace RadioMeti.Application.Services
{
    public class EventService: IEventService
    {
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly IGenericRepository<ArtistEvent> _artistEventRepository;
        private readonly IMapper _mapper;

        public EventService(IGenericRepository<ArtistEvent> artistEventRepository, IGenericRepository<Event> eventRepository, IMapper mapper)
        {
            _artistEventRepository = artistEventRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<Tuple<CreateEventResult,long>> CreateEvent(CreateEventDto create)
        {
            try
            {
                var newEvent = _mapper.Map<Event>(create);
               await _eventRepository.AddEntity(newEvent);
                await _eventRepository.SaveChangesAsync();
                return Tuple.Create(CreateEventResult.Success,newEvent.Id);
            }
            catch 
            {
                return Tuple.Create(CreateEventResult.Error,Convert.ToInt64(0));
            }
        }

        public async Task CreateEventArtists(long eventId, List<long> selectedArtists)
        {
            foreach (var item in selectedArtists)
                await _artistEventRepository.AddEntity(new ArtistEvent
                {
                    EventId= eventId,
                    ArtistId = item
                });
            await _artistEventRepository.SaveChangesAsync();
        }

        public async Task<DeleteEventResult> DeleteEvent(long id)
        {
            try
            {
                var deleteEvent = await _eventRepository.GetEntityById(id);
                if (deleteEvent == null) return DeleteEventResult.Notfound;
                _eventRepository.DeleteEntity(deleteEvent);
                await _eventRepository.SaveChangesAsync();
                return DeleteEventResult.Success;
            }
            catch
            {
                return DeleteEventResult.Error;
            }
        }

        public async Task DeleteEventArtists(long eventId)
        {
            foreach (var item in await _artistEventRepository.GetQuery().Where(p => p.EventId == eventId).ToListAsync())
                _artistEventRepository.DeleteEntity(item);
            await _artistEventRepository.SaveChangesAsync();
        }

        public async Task<EditEventResult> EditEvent(EditEventDto edit)
        {
            try
            {
                var eventEdited = await _eventRepository.GetEntityById(edit.Id);
                if (eventEdited == null) return EditEventResult.Notfound;
                eventEdited.UpdateDate = DateTime.Now;
                eventEdited.City= edit.City;
                eventEdited.Address= edit.Address;
                eventEdited.WhenOpen= edit.WhenOpen;
                eventEdited.AgeLimit= edit.AgeLimit;
                eventEdited.Cover= edit.Cover;
                eventEdited.HoldingDate= edit.HoldingDate;
                eventEdited.HoldingTime= edit.HoldingTime;
                eventEdited.Telephone= edit.Telephone;
                eventEdited.IsSlider= edit.IsSlider;
                eventEdited.InformationPhone= edit.InformationPhone;
                eventEdited.Description= edit.Description;
                eventEdited.Title= edit.Title;
                _eventRepository.EditEntity(eventEdited);
                await _eventRepository.SaveChangesAsync();
                return EditEventResult.Success;
            }
            catch
            {
                return EditEventResult.Error;
            }
        }

        public async Task<FilterEventDto> FilterEvent(FilterEventDto filter)
        {
            var query = _eventRepository.GetQuery().OrderByDescending(p => p.CreateDate).AsQueryable();
            #region State
            switch (filter.FilterEventState)
            {
                case FilterEventState.All:
                    break;
                case FilterEventState.InSlider:
                    query = query.Where(p => p.IsSlider).AsQueryable();
                    break;
                case FilterEventState.DosentInSlider:
                    query = query.Where(p => !p.IsSlider).AsQueryable();
                    break;
            }
            #endregion
            #region filter
            if (!string.IsNullOrEmpty(filter.Title)) query = query.Where(p => p.Title.Contains(filter.Title)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.HoldingTime)) query = query.Where(p => p.HoldingTime.Contains(filter.HoldingTime)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.Address)) query = query.Where(p => p.Address.Contains(filter.Address)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.Telephone)) query = query.Where(p => p.Telephone.Contains(filter.Telephone)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.City)) query = query.Where(p => p.City.Contains(filter.City)).AsQueryable();
            if (filter.AgeLimit != null) query = query.Where(p => p.AgeLimit == filter.AgeLimit).AsQueryable();
            if (!string.IsNullOrEmpty(filter.WhenOpen)) query = query.Where(p => p.WhenOpen.Contains(filter.WhenOpen)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.InformationPhone)) query = query.Where(p => p.InformationPhone.Contains(filter.InformationPhone)).AsQueryable();
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEvents= await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetEvents(allEvents).SetPaging(pager);
        }

        public async Task<List<long>> GetArtistsEvent(long eventId)
        {
            return await _artistEventRepository.GetQuery().Where(p => p.EventId == eventId).Select(p=>p.ArtistId).ToListAsync();
        }


        public async Task<Event> GetEventBy(long id)
        {
            return await _eventRepository.GetEntityById(id);
        }
    }
}
