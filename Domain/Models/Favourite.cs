using System;
using Domain.BaseModels;

namespace Domain.Models
{
    public class Favourite : IBaseEntity
    {
        public string FavouriteId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public BaseUser BaseUser { get; set; }
        public string UserId { get; set; }
    }
}