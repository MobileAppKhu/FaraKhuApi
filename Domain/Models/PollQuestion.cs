using System.Collections.Generic;

namespace Domain.Models;

public class PollQuestion : BaseEntity
{
    public string QuestionId { get; set; }
    public string QuestionDescription { get; set; }
    public ICollection<PollAnswer> Answers { get; set; }
    public bool MultiVote { get; set; } // Check if poll allows MultiVote
    public bool IsOpen { get; set; } // Check if poll is open (or closed)
    public string CourseId { get; set; }
    public Course Course { get; set; }
}