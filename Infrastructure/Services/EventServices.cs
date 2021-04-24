using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Interfaces.IServices;
using Domain.Models;
using Infrastructure.Persistence;

namespace Infrastructure.Services
{
    public class EventServices : IEventServices
    {
        private readonly DatabaseContext _context;

        public EventServices(DatabaseContext context)
        {
            _context = context;
        }
        
        public bool AddEvent(Event _event)
        {
            _context.Events.Add(_event);
            return SaveChanges();
        }

        public bool UpdateEvent(Event _event)
        {
            _context.Events.Update(_event);
            return SaveChanges();
        }

        public bool DeleteEvent(Event _event)
        {
            _context.Events.Remove(_event);
            return SaveChanges();
        }

        public ICollection<Event> UserEvents(string userId)
        {
            return _context.Events.Where(e => e.UserId == userId).ToList();
        }

        private bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}