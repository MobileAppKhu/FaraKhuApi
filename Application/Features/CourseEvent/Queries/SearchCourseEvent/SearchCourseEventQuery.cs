using Domain.Models;
using MediatR;

namespace Application.Features.CourseEvent.Queries.SearchCourseEvent;

public class SearchCourseEventQuery : IRequest<SearchCourseEventViewModel>
{
    public string CourseEventId { get; set; }
    public string EventName { get; set; }
    public string EventDescription { get; set; }
    public string CourseId { get; set; }
    public int Start { get; set; }
    public int Step { get; set; }
    public CourseEventColumn CourseEventColumn { get; set; }
    public bool OrderDirection { get; set; }
}