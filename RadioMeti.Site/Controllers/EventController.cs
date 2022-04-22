using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Event;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Site.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("/events")]
        public async Task<ActionResult> Index()
        {
            var model = new EventPageDto
            {
                Sliders = await _eventService.GetInSliderEvents(),
                NewestEvents = await _eventService.GetNewestEvents(25),
            };
            return View(model);
        }
    }
}
