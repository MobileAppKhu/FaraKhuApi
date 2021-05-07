﻿using Domain.Enum;
using MediatR;

namespace Application.Features.Poll.Commands.CreateAnswer
{
    public class CreateAnswerCommand : IRequest<CreateAnswerViewModel>
    {
        public string AnswerDescription { get; set; }
        public int QuestionId { get; set; }
    }
}