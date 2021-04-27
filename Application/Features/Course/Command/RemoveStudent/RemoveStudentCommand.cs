using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Course.Command.RemoveStudent
{
    public class RemoveStudentCommand : IRequest<RemoveStudentViewModel>
    {
        public int CourseId { get; set; }
        public string StudentId { get; set; }
    }
}
