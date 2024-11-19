using System.Text.Json;
using QuizApiBackend;
using QuizApiBackend.Models;

public class JsonQuestionRepository : IQuestionRepository
{
    private readonly string _filePath = Path.Combine("data", "questions.json");

    private List<Question> LoadData()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Question>();
        }

        var jsonData = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Question>>(jsonData) ?? new List<Question>();
    }

    private void SaveData(List<Question> questions)
    {
        var jsonData = JsonSerializer.Serialize(questions);
        File.WriteAllText(_filePath, jsonData);
    }

    public async Task<IEnumerable<Question>> GetAllAsync()
    {
        return await Task.Run(() => LoadData());
    }

    public async Task<Question> GetByIdAsync(int id)
    {
        var questions = LoadData();
        return await Task.Run(() => questions.FirstOrDefault(q => q.QnId == id));
    }

    public async Task<Question> AddAsync(Question question)
    {
        var questions = LoadData();
        question.QnId = questions.Count + 1; // Simple ID generation
        questions.Add(question);
        SaveData(questions);
        return await Task.FromResult(question);
    }

    public async Task UpdateAsync(Question question)
    {
        var questions = LoadData();
        var existingQuestion = questions.FirstOrDefault(q => q.QnId == question.QnId);
        if (existingQuestion != null)
        {
            questions.Remove(existingQuestion);
            questions.Add(question);
            SaveData(questions);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var questions = LoadData();
        var questionToRemove = questions.FirstOrDefault(q => q.QnId == id);
        if (questionToRemove != null)
        {
            questions.Remove(questionToRemove);
            SaveData(questions);
        }
    }

    public async Task<IEnumerable<Question>> GetByCategoryAsync(string category)
    {
        var questions = LoadData();
        return await Task.Run(() => questions.Where(q => q.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList());
    }
}

