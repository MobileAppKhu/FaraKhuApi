using System.Collections.Generic;
using Domain.BaseModels;

namespace Domain.Models;

public class Student : BaseUser
{
    public string StudentId { get; set; }
    public ICollection<Course> Courses { get; set; }
    public ICollection<PollAnswer> PollAnswers { get; set; }
}