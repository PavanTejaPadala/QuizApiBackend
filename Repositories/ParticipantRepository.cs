using Microsoft.EntityFrameworkCore;
using QuizApiBackend.Models;

public class ParticipantRepository : IRepository<Participant>
{
    private readonly QuizDbContext _context;

    public ParticipantRepository(QuizDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Participant>> GetAllAsync()
    {
        return await _context.Participants.ToListAsync();
    }

    public async Task<Participant> GetByIdAsync(int id)
    {
        return await _context.Participants.FindAsync(id);
    }

    public async Task<Participant> AddAsync(Participant participant)
    {
        await _context.Participants.AddAsync(participant);
        await _context.SaveChangesAsync();
        return participant;
    }

    public async Task UpdateAsync(Participant participant)
    {
        _context.Participants.Update(participant);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var participant = await GetByIdAsync(id);
        if (participant != null)
        {
            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
        }
    }
}
