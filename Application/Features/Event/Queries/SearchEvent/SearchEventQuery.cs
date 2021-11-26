using System;
using System.Collections.Generic;
using Domain.Enum;
using MediatR;

namespace Application.Features.Event.Queries.SearchEvent
{
    public class SearchEventQuery : IRequest<SearchEventViewModel>
    {
        public List<string> EventIds { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateTime? EventTime { get; set; }
        public int Start { get; set; }
        public int Step { get; set; }
        public EventColumn EventColumn { get; set; }
        public bool OrderDirection { get; set; }
    }
}