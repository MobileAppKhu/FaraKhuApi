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
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public UserType UserType { get; set; }
        public string ValidationCode { get; set; }
        public bool IsValidating { get; set; }
        public bool ResettingPassword { get; set; }
        public FileEntity Avatar { get; set; }
        public string AvatarId { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<Offer> Offers { get; set; }
        public ICollection<Suggestion> Suggestions { get; set; }
        public ICollection<Favourite> Favourites { get; set; }
        public ICollection<Announcement> Announcements { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}