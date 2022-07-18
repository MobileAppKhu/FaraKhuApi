using System.Collections.Generic;

namespace Domain.Models;

public class PollAnswer : BaseEntity
{
    public string AnswerId { get; set; }
    public string AnswerDescription { get; set; }
    public string QuestionId { get; set; }
    public PollQuestion Question { get; set; }
    public ICollection<Student> Voters { get; set; }
        
}