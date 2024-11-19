using QuizApiBackend.Models;

namespace QuizApiBackend
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<IEnumerable<Question>> GetByCategoryAsync(string category);
    }
}
