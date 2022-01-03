﻿using MediatR;

namespace Application.Features.Event.Queries.GetIncomingEvent
{
    public class GetIncomingEventQuery : IRequest<GetIncomingEventViewModel>
    {
        public int Start { get; set; }
        public int Step { get; set; }
    }
}