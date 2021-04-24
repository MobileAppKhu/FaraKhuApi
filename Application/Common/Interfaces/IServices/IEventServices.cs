using System.Collections.Generic;
using Domain.Models;

namespace Application.Common.Interfaces.IServices
{
    public interface IEventServices
    {
        public bool AddEvent(Event _event);
        public bool UpdateEvent(Event _event);
        public bool DeleteEvent(Event _event);
        public ICollection<Event> UserEvents(string userId);

    }
}