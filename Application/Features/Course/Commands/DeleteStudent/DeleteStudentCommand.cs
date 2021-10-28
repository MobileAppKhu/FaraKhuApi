using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Course.Commands.DeleteStudent
{
    public class DeleteStudentCommand : IRequest<DeleteStudentViewModel>
    {
        public string CourseId { get; set; }
        public string StudentId { get; set; }
    }
}
