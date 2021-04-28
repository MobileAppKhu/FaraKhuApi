using System;
using System.Collections.Generic;
using Domain.Enum;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.BaseModels
{
    public class BaseUser : IdentityUser, IBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Event> Events { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public UserType UserType { get; set; }
    }
}