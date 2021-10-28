﻿using MediatR;

namespace Application.Features.Poll.Commands.EditQuestion
{
    public class EditQuestionCommand : IRequest<Unit>
    {
        public string QuestionId { get; set; }
        public string QuestionDescription { get; set; }
        public string MultiVote { get; set; } // Check if poll allows MultiVote
        public string IsOpen { get; set; } // Check if poll is open (or closed)
    }
}