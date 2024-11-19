using Microsoft.AspNetCore.Mvc;
using QuizApiBackend;
using QuizApiBackend.Models;

[Route("api/[controller]")]
[ApiController]
public class QuestionController : Controller
{
    private readonly IQuestionRepository _questionRepository;

    public QuestionController(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    [HttpGet("ByCategory/{category}")]
    public async Task<IActionResult> GetQuestionsByCategory(string category)
    {
        var questions = await _questionRepository.GetByCategoryAsync(category);

        var selectedQuestions = questions
            .Select(x => new
            {
                x.QnId,
                x.QnInWords,
                x.ImageName,
                Options = new string[] { x.Option1, x.Option2, x.Option3, x.Option4 }
            })
            .OrderBy(y => Guid.NewGuid())
            .Take(10)
            .ToList();

        if (!selectedQuestions.Any())
        {
            return NotFound();
        }

        return Ok(selectedQuestions);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var question = await _questionRepository.GetByIdAsync(id.Value);
        if (question == null)
        {
            return NotFound();
        }

        return Ok(question);
    }

    [HttpPost]
    [Route("GetAnswers")]
    public async Task<IActionResult> RetrieveAnswers([FromBody] int[] qnIds)
    {
        var answers = (await _questionRepository.GetAllAsync())
            .Where(x => qnIds.Contains(x.QnId))
            .Select(y => new
            {
                y.QnId,
                y.QnInWords,
                y.ImageName,
                Options = new string[] { y.Option1, y.Option2, y.Option3, y.Option4 },
                y.Answer
            })
            .ToList();

        return Ok(answers);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(int id, [FromBody] Question question)
    {
        if (id != question.QnId)
        {
            return BadRequest();
        }

        await _questionRepository.UpdateAsync(question);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _questionRepository.DeleteAsync(id);
        return NoContent();
    }
}
