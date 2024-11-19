using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApiBackend.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Name { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Email { get; set; }
        public int Score { get; set; }

        public int TimeTaken { get; set; }
    }

    public class ParticipantResult
    {
        public int ParticipantID { get; set; }
        public int Score { get; set; }

        public int TimeTaken { get; set; }
    }
}
