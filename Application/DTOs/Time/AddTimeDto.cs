using System;
using Application.Common.Mappings;
using Domain.Enum;

namespace Application.DTOs.Time
{
    public class AddTimeDto
    {
        public WeekDay WeekDay { get; set; }

        public string StartTime { get; set; }

        //hh-mm
        public string EndTime { get; set; }
    }
}