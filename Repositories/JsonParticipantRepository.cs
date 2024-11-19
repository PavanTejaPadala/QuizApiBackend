using System.Text.Json;
using System.Text.Json.Serialization;
using QuizApiBackend.Models;

public class JsonParticipantRepository : IRepository<Participant>
{
    private readonly string _filePath = Path.Combine("data", "participants.json");

    private List<Participant> LoadData()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Participant>();
        }

        var jsonData = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Participant>>(jsonData) ?? new List<Participant>();
    }

    private void SaveData(List<Participant> participants)
    {
        var jsonData = JsonSerializer.Serialize(participants);
        File.WriteAllText(_filePath, jsonData);
    }

    public async Task<IEnumerable<Participant>> GetAllAsync()
    {
        return await Task.Run(() => LoadData());
    }

    public async Task<Participant> GetByIdAsync(int id)
    {
        var participants = LoadData();
        return await Task.Run(() => participants.FirstOrDefault(p => p.ParticipantID == id));
    }

    public async Task<Participant> AddAsync(Participant participant)
    {
        var participants = LoadData();
        participant.ParticipantID = participants.Count + 1;  // Simple ID generation
        participants.Add(participant);
        SaveData(participants);
        return await Task.FromResult(participant);
    }

    public async Task UpdateAsync(Participant participant)
    {
        var participants = LoadData();
        var existingParticipant = participants.FirstOrDefault(p => p.ParticipantID == participant.ParticipantID);
        if (existingParticipant != null)
        {
            participants.Remove(existingParticipant);
            participants.Add(participant);
            SaveData(participants);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var participants = LoadData();
        var participantToRemove = participants.FirstOrDefault(p => p.ParticipantID == id);
        if (participantToRemove != null)
        {
            participants.Remove(participantToRemove);
            SaveData(participants);
        }
    }
}
