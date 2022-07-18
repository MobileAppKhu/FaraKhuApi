using System;
using Domain.BaseModels;

namespace Domain.Models;

public class BaseEntity : IBaseEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public bool IsDeleted { get; set; }
}