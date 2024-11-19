using Microsoft.AspNetCore.Mvc;
using QuizApiBackend.Models;

[Route("api/[controller]")]
[ApiController]
public class ParticipantController : Controller
{
    private readonly IRepository<Participant> _participantRepository;

    public ParticipantController(IRepository<Participant> participantRepository)
    {
        _participantRepository = participantRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var participants = await _participantRepository.GetAllAsync();
        return Ok(participants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var participant = await _participantRepository.GetByIdAsync(id.Value);
        if (participant == null)
        {
            return NotFound();
        }

        return Ok(participant);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Participant participant)
    {
        // Check if a participant with the same name and email already exists
        var existingParticipant = (await _participantRepository.GetAllAsync())
            .FirstOrDefault(x => x.Name == participant.Name && x.Email == participant.Email);

        if (existingParticipant == null)
        {
            participant = await _participantRepository.AddAsync(participant);
        }
        else
        {
            participant = existingParticipant;
        }

        return Ok(participant);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(int id, [FromBody] ParticipantResult participantResult)
    {
        if (id != participantResult.ParticipantID)
        {
            return BadRequest();
        }

        var participant = await _participantRepository.GetByIdAsync(id);
        if (participant == null)
        {
            return NotFound();
        }

        participant.Score = participantResult.Score;
        participant.TimeTaken = participantResult.TimeTaken;

        await _participantRepository.UpdateAsync(participant);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _participantRepository.DeleteAsync(id);
        return NoContent();
    }
}
