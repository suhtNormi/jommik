using GymREST.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace GymREST.Data.Repos;
public class EventsRepo {
    private readonly DataContext _context;
    public EventsRepo(DataContext context)
    {
        _context = context;
    }
    public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task<Event> AddEventAsync(Event newEvent)
        {
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }

        public async Task<Event> UpdateEventAsync(Event updatedEvent)
        {
            _context.Events.Update(updatedEvent);
            await _context.SaveChangesAsync();
            return updatedEvent;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete == null) return false;

            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }

